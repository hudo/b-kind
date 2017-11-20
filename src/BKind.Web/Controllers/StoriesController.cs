using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class StoriesController : BkindControllerBase
    {
        private const string _ErrorKey = "__errors";
        private const string _InfoKey = "__info";

        public StoriesController(IMediator mediator) : base(mediator) {}

        public ActionResult Share() => View(new AddOrUpdateStoryInputModel());

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Share(AddOrUpdateStoryInputModel model)
        {
            model.UserId = (await GetLoggedUserAsync()).Id;

            if (!ModelState.IsValid)
                return View("Share", model);

            var response = await _mediator.Send(model);

            if (response.HasErrors)
            {
                MapToModelState(response);
                return View(model);
            }

            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new EditStoryInputModel();
            model.Story = await _mediator.Send(new GetByIdQuery<Story>(id));

            var user = await GetLoggedUserAsync();

            if (model.Story.AuthorId != user.GetRole<Author>()?.Id || model.Story == null) return NotFound();

            model.Title = $"Edit {model.Story.Title}";

            if(TempData.ContainsKey(_ErrorKey))
                model.Informations.Add((string)TempData[_ErrorKey]);

            return View(model);
        } 

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditStoryInputModel model)
        {
            var updateMessage = AddOrUpdateStoryInputModel.From(model.Story);
            var user = await GetLoggedUserAsync();
            updateMessage.UserId = user.Id;

            var response = await _mediator.Send(updateMessage);

            if (response.HasErrors)
            {
                foreach (var responseError in response.Errors)
                    ModelState.AddModelError(responseError.Key, responseError.Message);

                return View(model);
            }

            TempData[_ErrorKey] = "Story succesfully saved!";

            return RedirectToAction("Edit", new { id = response.Result.Id });
        }

        public async Task<IActionResult> Read(int id)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery { StoryId = id, UserWithRoles = user });

            if (stories == null || !stories.Any()) return NotFound();

            var model = new ReadStoryViewModel(stories[0], user);

            if (TempData.ContainsKey(_ErrorKey))
                model.Errors.Add((string)TempData[_ErrorKey]);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Publish(int id) => await ChangeStatus(id, Status.Published);

        [Authorize]
        public async Task<IActionResult> Unpublish(int id) => await ChangeStatus(id, Status.Draft);

        [Authorize]
        public async Task<IActionResult> ThumbsUp(int id)
        {
            var user = await GetLoggedUserAsync();

            var result = await _mediator.Send(new ThumbsUpCommand(user, id));
            
            return RedirectToAction("Read", new { id });
        }

        private async Task<IActionResult> ChangeStatus(int id, Status newStatus)
        {
            var user = await GetLoggedUserAsync();
            var response = await _mediator.Send(new ChangeStatusCommand(id, user, newStatus));

            if (response.HasErrors)
            {
                TempData[_ErrorKey] = string.Join(", ", response.Errors.Select(x => x.Message));
            }

            return RedirectToAction("Read", new { id });
        }
    }
}

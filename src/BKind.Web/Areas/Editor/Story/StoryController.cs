using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Areas.Editor.Story.Domain;
using BKind.Web.Areas.Editor.Story.Models;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Shared;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Features.Stories.Queries;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BKind.Web.Areas.Editor.Story
{
    [Area(Areas.Editor)]
    public class StoryController : BkindControllerBase
    {
        private readonly AppSettings _appSettings;

        public StoryController(IMediator mediator, IOptions<AppSettings> appSettings) : base(mediator)
        {
            _appSettings = appSettings.Value;
        }

        [Authorize]
        public ActionResult Share() => View(new AddOrUpdateStoryInputModel());

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
        public async Task<IActionResult> Edit(string id)
        {
            var model = new AddOrUpdateStoryInputModel();

            var story = await _mediator.Send(new GetStoryQuery(id));
            var user = await GetLoggedUserAsync();

            if (story == null)
                return NotFound("Story not found");

            if(story.AuthorId != user.GetRole<Author>()?.Id && !user.Is<Administrator>())
                return Unauthorized();

            var tags = await _mediator.Send(new GetAllQuery<Tag>(tag => tag.StoryTags.Any(st => st.StoryId == story.Id)));

            model.StoryId = story.Id;
            model.StoryTitle = story.Title;
            model.Content = story.Content;
            model.Tags = string.Join(',', tags.Select(x => x.Title));
            model.Title = $"Edit {story.Title}";

            if (!string.IsNullOrEmpty(story.Photo))
                model.ExistingImage = $"{_appSettings.StorageDomain}{_appSettings.StoryPhotoContainer}/{story.Photo}";

            if (TempData.ContainsKey(_ErrorKey))
                model.Informations.Add((string)TempData[_ErrorKey]);

            return View("Share", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(AddOrUpdateStoryInputModel model)
        {
            var user = await GetLoggedUserAsync();
            model.UserId = user.Id;

            if (!ModelState.IsValid)
                return View("Share", model);

            var response = await _mediator.Send(model);

            if (response.HasErrors)
            {
                MapToModelState(response);
                return View("Share", model);
            }

            TempData[_ErrorKey] = "Story succesfully saved!";

            return RedirectToAction("Edit", new { id = response.Result.Slug });
        }

        public async Task<IActionResult> Publish(string id) => await ChangeStatus(id, Status.Published);

        public async Task<IActionResult> Unpublish(string id) => await ChangeStatus(id, Status.Draft);

        private async Task<IActionResult> ChangeStatus(string id, Status newStatus)
        {
            var user = await GetLoggedUserAsync();
            var response = await _mediator.Send(new ChangeStatusCommand(id, user, newStatus));

            if (response.HasErrors)
            {
                TempData[_ErrorKey] = string.Join(", ", response.Errors.Select(x => x.Message));
            }
            
            return RedirectToAction("Read", "Stories", new { id, area = "" });
        }

        [Authorize]
        public async Task<IActionResult> Pin(string id)
        {
            var user = await GetLoggedUserAsync();
            await _mediator.Send(new PinStoryCommand(id, user));
            return Redirect("/");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await GetLoggedUserAsync();

            var response = await _mediator.Send(new DeleteStoryCommand(id, user));

            return Redirect("/");
        }
    }
}
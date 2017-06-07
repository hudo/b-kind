using System;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class StoriesController : BkindControllerBase
    {
        private const string TempDataSaveInfo = "__savedInfo";

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

            if(TempData.ContainsKey(TempDataSaveInfo))
                model.Informations.Add((string)TempData[TempDataSaveInfo]);

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

            TempData[TempDataSaveInfo] = "Story succesfully saved!";

            return RedirectToAction("Edit", new { id = response.Result.Id });
        }
    }
}

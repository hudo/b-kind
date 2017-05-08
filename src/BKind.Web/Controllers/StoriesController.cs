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
        public StoriesController(IMediator mediator) : base(mediator) {}

        public ActionResult Share() => View(new CreateStoryInputModel());

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Share(CreateStoryInputModel model)
        {
            model.UserId = LoggedUser().Id;

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

            var user = await LoggedUser();

            if (model.Story.AuthorId != user.Id || model.Story == null) return NotFound();

            model.Title = $"Edit {model.Story.Title}";

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IAsyncResult> Edit(EditStoryInputModel model)
        {
            
        }
    }
}

using System;
using System.Threading.Tasks;
using BKind.Web.Features.Stories;
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
    }
}

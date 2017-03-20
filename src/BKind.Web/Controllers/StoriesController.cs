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
        
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Share(CreateStoryInputModel model)
        {
            var user = await LoggedUser();
            model.UserId = user.Id;
            
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

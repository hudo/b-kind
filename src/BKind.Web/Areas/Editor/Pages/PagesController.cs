using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Account.Domain;
using BKind.Web.Features.Pages.Models;
using BKind.Web.Features.Shared;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Areas.Editor.Pages
{
    [AdminsOnly]
    [Area("Editor")]
    public class PagesController : BkindControllerBase
    {
        public PagesController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var model = new PageEditModel();

            if (id.HasValue)
            {
                model.Page = await _mediator.Send(new GetByIdQuery<Page>(id.Value));

                if (model.Page == null) return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.User = await GetLoggedUserAsync();

            var response = await _mediator.Send(model);

            if(response.HasErrors)
            {
                MapToModelState(response);
                return View(model);
            }

            return RedirectToAction("Edit", new { id=response.Result.Id });
        }
    }
}
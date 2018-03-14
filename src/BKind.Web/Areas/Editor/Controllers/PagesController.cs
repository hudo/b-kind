using System.Threading.Tasks;
using BKind.Web.Controllers;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Pages.Models;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Areas.Editor.Controllers
{
    [Authorize]
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
            return View();
        }
    }
}
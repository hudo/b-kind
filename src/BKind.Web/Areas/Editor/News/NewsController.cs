using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Account.Domain;
using BKind.Web.Features.News.Commands;
using BKind.Web.Features.News.Models;
using BKind.Web.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Areas.Editor.News
{
    [Area(Areas.Editor)]
    [AdminsOnly]
    public class NewsController : BkindControllerBase
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var model = new NewsInputModel { News = new Model.News() };

            if(id.HasValue)
            {
                model.News = await _mediator.Send(new GetByIdQuery<Model.News>(id.Value));
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(NewsInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(model);

            if(result.HasErrors)
            {
                MapToModelState(result);
                return View(model);
            }

            return Redirect("/");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteNewsCommand(id, await GetLoggedUserAsync()));

            if (result.HasErrors)
                return BadRequest(string.Join(", ", result.Errors.Select(x => x.Message)));

            return Redirect("/");
        }
    }
}
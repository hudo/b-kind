using System.Threading.Tasks;
using BKind.Web.Controllers;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.News.Models;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Areas.Editor.Controllers
{
    [Authorize]
    [Area(Areas.Editor)]
    public class NewsController : BkindControllerBase
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var model = new NewsInputModel { News = new News() };

            if(id.HasValue)
            {
                model.News = await _mediator.Send(new GetByIdQuery<News>(id.Value));
            }

            return View(model);
        }
    }
}
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class NewsController : BkindControllerBase
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> Index(string slug)
        {
            var news = await _mediator.Send(new GetOneQuery<News>(x => x.Slug == slug));

            if (news == null)
                return NotFound($"News item '{slug}' not found!");

            return View(news);
        }
    }
}
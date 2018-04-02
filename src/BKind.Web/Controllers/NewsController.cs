using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using BKind.Web.ViewModels;
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

            var user = await GetLoggedUserAsync();
            var canEdit = user != null && user.Is<Administrator>();

            var model = new NewsModel(news, canEdit);

            return View(model);
        }
    }
}
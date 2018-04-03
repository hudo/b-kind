using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.News.Models;
using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.News
{
    public class NewsController : BkindControllerBase
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> Index(string slug)
        {
            var news = await _mediator.Send(new GetOneQuery<Model.News>(x => x.Slug == slug));

            if (news == null)
                return NotFound($"News item '{slug}' not found!");

            var user = await GetLoggedUserAsync();
            var canEdit = user != null && user.Is<Administrator>();

            var model = new NewsModel(news, canEdit);

            return View(model);
        }
    }
}
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Pages
{
    public class PagesController : BkindControllerBase
    {
        public PagesController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> Index(string slug)
        {
            var page = await _mediator.Send(new GetOneQuery<Page>(x => x.Slug == slug.ToLower()));

            if (page == null) return NotFound($"Page {slug} not found");

            var user = await GetLoggedUserAsync();

            return View(new PageModel(page, user));
        }
    }
}
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BKind.Web.Controllers
{
    public class HomeController : BkindControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(IMediator mediator, ILogger<HomeController> logger) : base(mediator)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel
            {
                Title = "Welcome to Be Kind",
                CanWriteStory = User.Identity.IsAuthenticated,
                Latest = new StoryListModel(
                    await _mediator.Send(new ListStoriesQuery { Paging = new PagedOptions<Story>(orderBy: s => s.Modified, ascending: false)}),
                    await GetLoggedUserAsync()),
                Best = new StoryListModel(
                    await _mediator.Send(new ListStoriesQuery { Paging = new PagedOptions<Story>(orderBy: s => s.Views, ascending: false)}),
                    await GetLoggedUserAsync())
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
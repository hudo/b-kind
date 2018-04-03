using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Home.Models;
using BKind.Web.Features.Shared;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Features.Stories.Queries;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BKind.Web.Features.Home
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
            var user = await GetLoggedUserAsync();

            var model = new HomePageViewModel
            {
                Title = "Welcome to Be Kind",
                CanWriteStory = User.Identity.IsAuthenticated,
                Latest = new StoryListModel(
                    await _mediator.Send(new ListStoriesQuery
                    {
                        UserWithRoles = user, 
                        Paging = new PagedOptions<Story>(pageSize: 5, orderBy: s => s.Modified, @ascending: false),
                        Pinned = false
                    }),
                    user),
                Recommended = new StoryListModel(
                    await _mediator.Send(new ListStoriesQuery
                    {
                        UserWithRoles = user, 
                        Paging = new PagedOptions<Story>(pageSize: 5, orderBy: s => s.Views, @ascending: false),
                        Pinned = true
                    }),
                    user)
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
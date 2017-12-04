using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Controllers
{
    public class StoriesController : BkindControllerBase
    {
        public StoriesController(IMediator mediator) : base(mediator) {}

        public async Task<IActionResult> Read(int id)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery { StoryId = id, UserWithRoles = user, IncludeTags = true});

            if (stories == null || !stories.Any()) return NotFound();

            var model = new ReadStoryViewModel(stories[0], user);

            await _mediator.Send(new IncreaseStoryViewCountCommand(stories[0].Id));

            if (TempData.ContainsKey(_ErrorKey))
                model.Errors.Add((string)TempData[_ErrorKey]);

            return View(model);
        }

        public async Task<IActionResult> Tag(string id)
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ThumbsUp(int id)
        {
            var user = await GetLoggedUserAsync();

            var result = await _mediator.Send(new ThumbsUpCommand(user, id));

            return RedirectToAction("Read", new { id });
        }
    }
}

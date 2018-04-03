using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Shared;
using BKind.Web.Features.Stories.Commands;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Features.Stories.Queries;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Stories
{
    public class StoriesController : BkindControllerBase
    {
        public StoriesController(IMediator mediator) : base(mediator) {}

        public async Task<IActionResult> Read(string id)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery { StorySlug = id, UserWithRoles = user, IncludeTags = true});

            if (stories == null || !stories.Any()) return NotFound();

            var model = new ReadStoryViewModel(stories[0], user);

            await _mediator.Send(new IncreaseStoryViewCountCommand(stories[0].Id));

            if (TempData.ContainsKey(_ErrorKey))
                model.Errors.Add((string)TempData[_ErrorKey]);

            return View(model);
        }

        public async Task<IActionResult> List(bool? recommended, string tag, string author, int? page)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery
            {
                UserWithRoles = user,
                Paging = new PagedOptions<Story>(page, orderBy: x => x.Modified, @ascending: false),
                Pinned = recommended,
                Tag = tag,
                AuthorNick = author,
            });

            var title = recommended.HasValue
                ? "recommended"
                : !string.IsNullOrEmpty(tag)
                    ? $"by tag '{tag}'"
                    : "all";

            return View(new StoryListModel(stories, user, $"Browse {title}"));
        }

        [Authorize]
        public async Task<IActionResult> ThumbsUp(string id)
        {
            var user = await GetLoggedUserAsync();

            var result = await _mediator.Send(new ThumbsUpCommand(user, id));

            return RedirectToAction("Read", new { id });
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using BKind.Web.Core;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Shared;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Features.Stories.Models;
using BKind.Web.Features.Stories.Queries;
using BKind.Web.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BKind.Web.Features.Stories
{
    public class StoriesController : BkindControllerBase
    {
        private readonly AppSettings _appSetting;

        public StoriesController(IMediator mediator, IOptions<AppSettings> appSetting) : base(mediator)
        {
            _appSetting = appSetting.Value;
        }

        public async Task<IActionResult> Read(string slug)
        {
            var user = await GetLoggedUserAsync();
            var stories = await _mediator.Send(new ListStoriesQuery { StorySlug = slug, UserWithRoles = user, IncludeTags = true});

            if (stories == null || !stories.Any()) return NotFound();

            var model = new ReadStoryViewModel(stories[0], user);
            model.PhotoUrl = $"{_appSetting.StorageDomain}{_appSetting.StoryPhotoContainer}/{stories[0].Photo}";

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
                Paging = new PagedOptions<Story>(page, orderBy: x => x.Created, @ascending: false),
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

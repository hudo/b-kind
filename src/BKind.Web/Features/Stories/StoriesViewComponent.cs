using System.Threading.Tasks;
using BKind.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using Story = BKind.Web.Model.Story;

namespace BKind.Web.Features.Stories
{
    public class StoriesViewComponent : ViewComponent
    {
        readonly IMediator _mediator;

        public StoriesViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(StoriesDisplayMode mode)
        {
            if (mode == StoriesDisplayMode.WriteNew)
                return View("Write");

            var model = new StoryListViewModel();

            if (User.Identity.IsAuthenticated)
                model.UserWithRoles = await _mediator.Send(new GetOneQuery<User>(x => x.Username == User.Identity.Name, u => u.Roles));

            model.Stories = await _mediator.Send(new ListStoriesQuery { UserWithRoles = model.UserWithRoles });
            
            return View(model);
        }
    }
}
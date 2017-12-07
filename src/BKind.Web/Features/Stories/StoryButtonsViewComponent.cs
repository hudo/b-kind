using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Model;
using Microsoft.AspNetCore.Mvc;

namespace BKind.Web.Features.Stories
{
    public class StoryButtonsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(StoryProjection story, User userWithRoles)
        {
            if (userWithRoles == null)
                return View("Anonymous");

            var model = new StoryButtonsViewModel();

            model.Slug = story.Slug;
            model.CanEdit = userWithRoles.GetRole<Author>()?.Id == story.AuthorId;
            model.CanUnpublish = story.Status == Status.Published && (model.CanEdit || userWithRoles.Is<Administrator>() || userWithRoles.Is<Reviewer>());
            model.CanVote = !model.CanEdit;
            model.CanPublish = (userWithRoles.Is<Reviewer>() || userWithRoles.Is<Administrator>()) && story.Status == Status.Draft;
            
            model.CanPin = userWithRoles.Is<Administrator>() && !story.Pinned;
            model.CanUnpin = userWithRoles.Is<Administrator>() && story.Pinned;

            model.CanDelete = userWithRoles.Is<Administrator>();

            model.Slug = story.Slug;

            return View(model);
        }
    }
}
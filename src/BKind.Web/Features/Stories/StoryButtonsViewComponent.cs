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

            model.CanEdit = userWithRoles.GetRole<Author>()?.Id == story.AuthorId;
            model.CanUnpublish = story.Status == Status.Published && (model.CanEdit || userWithRoles.Is<Administrator>() || userWithRoles.Is<Reviewer>());
            model.CanVote = !model.CanEdit;

            model.StoryId = story.Id;

            return View(model);
        }
    }
}
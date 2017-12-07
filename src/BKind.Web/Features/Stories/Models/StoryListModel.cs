using System.Collections.Generic;
using BKind.Web.Model;
using BKind.Web.ViewModels;

namespace BKind.Web.Features.Stories.Contracts
{
    public class StoryListModel : ViewModelBase
    {
        public StoryListModel(IList<StoryProjection> stories, User userWithRoles, string title = null)
        {
            Stories = stories;
            UserWithRoles = userWithRoles;
            Title = title;
        }

        public IList<StoryProjection> Stories { get; }
        public User UserWithRoles { get; }

        public string Title { get; set; }
    }
}
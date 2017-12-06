using System.Collections.Generic;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories.Contracts
{
    public class StoryListModel
    {
        public StoryListModel(IList<StoryProjection> stories, User userWithRoles)
        {
            Stories = stories;
            UserWithRoles = userWithRoles;
        }

        public IList<StoryProjection> Stories { get; }
        public User UserWithRoles { get; }
    }
}
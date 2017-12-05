using System.Collections.Generic;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories.Contracts
{
    public class StoryListModel
    {
        public StoryListModel(IEnumerable<StoryProjection> stories, User userWithRoles)
        {
            Stories = stories;
            UserWithRoles = userWithRoles;
        }

        public IEnumerable<StoryProjection> Stories { get; set; }
        public User UserWithRoles { get; set; }
    }
}
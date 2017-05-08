using System.Collections.Generic;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories
{
    public class StoryListViewModel
    {
        public IEnumerable<StoryProjection> Stories { get; set; }
        public User UserWithRoles { get; set; }
    }
}
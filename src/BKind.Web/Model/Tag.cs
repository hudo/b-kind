using System.Collections.Generic;

namespace BKind.Web.Model
{
    public class Tag : Entity
    {
        public string Title { get; set; }

        public IList<StoryTags> StoryTags { get; set; }
    }
}
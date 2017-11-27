using System.Collections.Generic;

namespace BKind.Web.Model
{
    public class Tag : Identity
    {
        public string Title { get; set; }

        public IList<StoryTags> StoryTags { get; set; }
    }
}
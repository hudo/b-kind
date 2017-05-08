using System;

namespace BKind.Web.Features.Stories
{
    public class StoryProjection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ThumbsUp { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public DateTime Created { get; set; }
    }
}
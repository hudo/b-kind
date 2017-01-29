using System;

namespace BKind.Web.Model
{
    public class Story : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Status Status { get; set; }
        public int ThumbsUp { get; set; }
        public DateTime Deleted { get; set; }
    }
}
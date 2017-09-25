using System;

namespace BKind.Web.Model
{
    public class StoryVotes : Entity
    {
        public int StoryId { get; set; }
        public Story Story { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime Voted { get; set; }
    }
}
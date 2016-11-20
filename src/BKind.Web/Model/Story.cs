using System;
using System.Collections.Generic;

namespace BKind.Web.Model
{
    public class Story : Entity
    {
        public string Title { get; set; }
        public string Content { get; set; }
     
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Status Status { get; set; }
        public int ThumbsUp { get; set; }
        public DateTime Deleted { get; set; }
    }


    public enum Status
    {
        Draft, Published, Declined
    }

    public abstract class Entity
    {
        public int ID { get; set; }
    }
}
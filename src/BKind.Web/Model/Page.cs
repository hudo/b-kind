using System;

namespace BKind.Web.Model
{
    public class Page : Identity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }

        public DateTimeOffset Published { get; set; }
        public DateTimeOffset Modified { get; set; }
        public int ModifiedBy { get; set; }
        public User ModifiedByUser { get; set; }
        public int CreatedBy { get; set; }
        public User CreatedByUser { get; set; }
    }
}
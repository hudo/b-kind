using System;

namespace BKind.Web.Model
{
    public class News : Identity
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public DateTime Published { get; set; }
    }
}
using System.Collections.Generic;

namespace BKind.Web.Features.News
{
    public class NewsWidgetModel
    {
        public IEnumerable<Model.News> Newses { get; set; }

        public bool CanAdd { get; set; }
    }
}
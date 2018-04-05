using BKind.Web.Features.Shared;

namespace BKind.Web.Features.News
{
    public class NewsModel : ViewModelBase
    {
        public NewsModel(Model.News news, bool canEdit)
        {
            News = news;
            CanEdit = canEdit;
        }

        public Model.News News { get; set; }
        public bool CanEdit { get; set; }
    }
}
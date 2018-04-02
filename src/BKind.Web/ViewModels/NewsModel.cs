using BKind.Web.Model;

namespace BKind.Web.ViewModels
{
    public class NewsModel : ViewModelBase
    {
        public NewsModel(News news, bool canEdit)
        {
            News = news;
            CanEdit = canEdit;
        }

        public News News { get; set; }
        public bool CanEdit { get; set; }
    }
}
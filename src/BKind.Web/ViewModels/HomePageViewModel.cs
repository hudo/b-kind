using BKind.Web.Features.Stories.Contracts;

namespace BKind.Web.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {

        }

        public bool CanWriteStory { get;set; }
        public StoryListModel Latest { get; set; }
        public StoryListModel Best { get; set; }
    }
}
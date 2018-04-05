using BKind.Web.Features.Shared;
using BKind.Web.Features.Stories.Contracts;

namespace BKind.Web.Features.Home
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {

        }

        public bool CanWriteStory { get;set; }
        public StoryListModel Latest { get; set; }
        public StoryListModel Recommended { get; set; }
    }
}
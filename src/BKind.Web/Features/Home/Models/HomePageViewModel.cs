using BKind.Web.Features.Shared;
using BKind.Web.Features.Stories.Contracts;
using BKind.Web.Infrastructure;

namespace BKind.Web.Features.Home.Models
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
using System.Collections.Generic;
using BKind.Web.Model;

namespace BKind.Web.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {
            FeaturedStories = new List<Story>();
        }

        public List<Story> FeaturedStories { get; set; }
    }
}
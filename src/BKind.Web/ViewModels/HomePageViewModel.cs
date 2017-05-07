namespace BKind.Web.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {

        }

        public StoriesDisplayMode DisplayMode { get; set; }
        public bool CanWriteStory { get;set; }
    }
    
    public enum StoriesDisplayMode
    {
        None,
        FeaturedList,
        WriteNew
    }
}
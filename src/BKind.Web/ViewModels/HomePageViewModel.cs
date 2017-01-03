namespace BKind.Web.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {

        }

        public StoriesDisplayMode DisplayMode { get; set; }

        public string FirstName { get; set; }
    }

    public class HomeInputModel
    {
        public string FirstName { get; set; }
    }

    public enum StoriesDisplayMode
    {
        Featured,
        Latest
    }
}
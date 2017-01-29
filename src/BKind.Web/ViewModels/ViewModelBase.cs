namespace BKind.Web.ViewModels
{
    public abstract class ViewModelBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = $"{value} | B Kind"; }
        }
        public string Description { get; set; }
    }
}
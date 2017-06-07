using System.Collections.Generic;

namespace BKind.Web.ViewModels
{
    public abstract class ViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => _title = $"{value} | B Kind";
        }
        public string Description { get; set; }
    }

    /// <summary>
    /// View model that contains additional UI messages 
    /// </summary>
    public abstract class FormModelBase : ViewModelBase
    {
        public FormModelBase()
        {
            Informations = new List<string>();
            Errors = new List<string>();
        }

        public List<string> Informations { get; set; }
        public List<string> Errors { get; set; }
        
    }
}
using System.Collections.Generic;

namespace BKind.Web.ViewModels
{
    public abstract class ViewModelBase
    {
        public ViewModelBase()
        {
            Informations = new List<string>();    
            Errors = new List<string>();
        }

        private string _title;

        public string Title
        {
            get => _title;
            set => _title = $"{value} | B Kind";
        }

        public string BodyClass { get; set; }
        public string Description { get; set; }

        public List<string> Informations { get; set; }
        public List<string> Errors { get; set; }
    }



    public class FormModel<T> : ViewModelBase
    {
        public FormModel(T model)
        {
            Model = model;
        }

        public T Model { get; set; }
    }
}
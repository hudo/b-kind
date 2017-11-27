using BKind.Web.Model;
using BKind.Web.ViewModels;

namespace BKind.Web.Features.Stories.Contracts
{
    public class EditStoryInputModel : ViewModelBase
    {
        public Story Story { get; set; }

        public string Tags { get; set; }
    }
}
using BKind.Web.Model;
using BKind.Web.ViewModels;

namespace BKind.Web.Features.Stories
{
    public class EditStoryInputModel : FormModelBase
    {
        public Story Story { get; set; }
    }
}
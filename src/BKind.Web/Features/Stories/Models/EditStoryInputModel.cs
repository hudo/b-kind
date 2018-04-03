using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using BKind.Web.Model;

namespace BKind.Web.Features.Stories.Contracts
{
    public class EditStoryInputModel : ViewModelBase
    {
        public Story Story { get; set; }

        public string Tags { get; set; }
    }
}
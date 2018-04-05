using BKind.Web.Core;
using BKind.Web.Features.Shared;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Areas.Editor.Pages.Models
{
    public class PageEditModel : ViewModelBase, IRequest<Response<Page>>
    {
        public PageEditModel()
        {
            Page = new Page();    
        }

        public Page Page { get; set; }
        public User User { get; set; }
    }
}
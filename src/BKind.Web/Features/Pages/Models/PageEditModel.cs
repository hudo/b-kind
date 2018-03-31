using BKind.Web.Core;
using BKind.Web.Model;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Pages.Models
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
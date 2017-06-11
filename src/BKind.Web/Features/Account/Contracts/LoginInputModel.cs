using BKind.Web.Core;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Account.Contracts
{
    public class LoginInputModel : ViewModelBase, IRequest<Response>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
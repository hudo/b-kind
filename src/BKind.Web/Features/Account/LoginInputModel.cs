using BKind.Web.Infrastructure;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Account
{
    public class LoginInputModel : ViewModelBase, IAsyncRequest<Response<bool>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
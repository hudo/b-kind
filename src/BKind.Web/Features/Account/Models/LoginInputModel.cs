using System.ComponentModel;
using BKind.Web.Core;
using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using MediatR;

namespace BKind.Web.Features.Account.Contracts
{
    public class LoginInputModel : ViewModelBase, IRequest<Response>
    {
        [DisplayName("E-mail")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
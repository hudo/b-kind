using System.ComponentModel.DataAnnotations;
using BKind.Web.Core;
using BKind.Web.ViewModels;
using MediatR;

namespace BKind.Web.Features.Account
{
    public class RegisterInputModel : ViewModelBase, IAsyncRequest<Response<bool>>
    {
        [Display(Name = "E-mail")]
        public string Username { get; set; }

        [Display(Name = "First name")]
        public string Firstname { get; set; }

        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
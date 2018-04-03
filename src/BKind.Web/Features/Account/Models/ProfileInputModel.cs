using System.ComponentModel.DataAnnotations;
using BKind.Web.Core;
using BKind.Web.Features.Shared;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using MediatR;

namespace BKind.Web.Features.Account.Contracts
{
    public class ProfileInputModel : ViewModelBase, IRequest<Response>
    {
        public ProfileInputModel()
        {
            
        }

        public ProfileInputModel(User user)
        {
            Username = user.Username;
            Firstname = user.FirstName;
            Lastname = user.LastName;
        }


        [Display(Name = "E-mail*")]
        public string Username { get; set; }

        [Display(Name = "Nick name*")]
        public string Nick { get; set; }

        [Display(Name = "First name")]
        public string Firstname { get; set; }

        [Display(Name = "Last name")]
        public string Lastname { get; set; }

        [Display(Name = "Password*")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
using BKind.Web.ViewModels;

namespace BKind.Web.Features.Account
{
    public class RegisterInputModel : ViewModelBase
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
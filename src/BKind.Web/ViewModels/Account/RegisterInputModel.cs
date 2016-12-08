using System.ComponentModel.DataAnnotations;

namespace BKind.Web.ViewModels.Account
{
    public class RegisterInputModel : ViewModelBase
    {
        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
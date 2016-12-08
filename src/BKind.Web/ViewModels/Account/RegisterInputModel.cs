using System.ComponentModel.DataAnnotations;

namespace BKind.Web.ViewModels.Account
{
    public class RegisterInputModel : ViewModelBase
    {
        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
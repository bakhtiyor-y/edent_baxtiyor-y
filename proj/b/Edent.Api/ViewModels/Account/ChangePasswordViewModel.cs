using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [MinLength(6)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6)]
        [Compare("ConfirmNewPassword")]
        public string NewPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string ConfirmNewPassword { get; set; }
    }
}

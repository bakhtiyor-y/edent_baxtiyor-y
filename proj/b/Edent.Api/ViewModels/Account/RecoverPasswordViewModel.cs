using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels.Account
{
    public class RecoverPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string ResetToken { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation mismatch")]
        public string ConfirmPassword { get; set; }
    }
}

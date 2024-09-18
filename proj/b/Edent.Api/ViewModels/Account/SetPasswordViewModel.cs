using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels.Account
{
    public class SetPasswordViewModel
    {
        public int UserId { get; set; }

        [Required]
        [MinLength(6)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}

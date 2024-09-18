using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName required field")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password required field")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

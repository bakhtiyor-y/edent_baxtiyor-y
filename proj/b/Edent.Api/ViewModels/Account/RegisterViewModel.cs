using System;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation mismatch")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset BirthDate { get; set; }
        public string Patronymic { get; set; }
        public string AuthRequestToken { get; set; }
    }
}

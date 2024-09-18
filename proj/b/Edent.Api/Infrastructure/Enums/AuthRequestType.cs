using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Enums
{
    public enum AuthRequestType
    {
        [Display(Name = "Registration")]
        RegisterUser = 0,
        [Display(Name = "Recover")]
        RecoverPassword = 1,
        [Display(Name = "Confirm")]
        AccessPatientData = 2,
        [Display(Name = "Change email")]
        ChangeEmail = 3,
        [Display(Name = "Change phone")]
        ChangePhone = 4
    }
}

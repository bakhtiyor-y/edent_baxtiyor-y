using Edent.Api.Infrastructure.Enums;
using System;

namespace Edent.Api.ViewModels
{
    public class PatientViewModel : IViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public PatientAgeType PatientAgeType { get; set; }
        public Gender Gender { get; set; }
    }
}

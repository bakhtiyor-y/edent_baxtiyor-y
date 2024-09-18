using System;
using System.ComponentModel.DataAnnotations;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels
{
    public class PatientManageViewModel : IViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Patronymic { get; set; }

        public string FullName { get => $"{LastName} {FirstName} {Patronymic}"; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }

        [StringLength(512)]
        public string AddressLine1 { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public PatientAgeType PatientAgeType { get; set; }
        public Gender Gender { get; set; }
    }
}

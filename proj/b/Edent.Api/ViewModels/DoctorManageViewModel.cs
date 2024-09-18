using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels
{
    public class DoctorManageViewModel : IViewModel
    {
        public DoctorManageViewModel()
        {
            DentalChairs = new HashSet<int>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Patronymic { get; set; }

        public int SpecializationId { get; set; }
        public int TermId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double TermValue { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset BirthDate { get; set; }

        [Required]
        [StringLength(512)]
        public string AddressLine1 { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public ICollection<int> DentalChairs { get; set; }
        public Gender Gender { get; set; }
    }
}

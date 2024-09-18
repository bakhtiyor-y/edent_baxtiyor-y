using System;
using System.ComponentModel.DataAnnotations;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels
{
    public class DoctorViewModel : IViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Patronymic { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImage { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
        public string Specialization { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Term { get; set; }
        public Gender Gender { get; set; }
    }
}

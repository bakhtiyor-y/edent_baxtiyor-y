using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels.Account
{
    public class UserManageViewModel : IViewModel
    {
        public UserManageViewModel()
        {
            Roles = new HashSet<string>();
        }
        public int Id { get; set; }

        public int? EmployeeId { get; set; }


        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Patronymic { get; set; }

        [Required]
        public DateTimeOffset BirthDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string ProfileImage { get; set; }
        public Gender Gender { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}

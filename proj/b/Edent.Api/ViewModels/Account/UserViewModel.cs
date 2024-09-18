using System;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels.Account
{
    public class UserViewModel : IViewModel
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
    }
}

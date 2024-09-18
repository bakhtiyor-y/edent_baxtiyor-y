using System;
using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels
{
    public class TechnicViewModel : IViewModel
    {
        public int Id { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
    }
}

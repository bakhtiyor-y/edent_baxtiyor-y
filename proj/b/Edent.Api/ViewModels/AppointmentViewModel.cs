using Edent.Api.Infrastructure.Enums;
using System;

namespace Edent.Api.ViewModels
{
    public class AppointmentViewModel : IViewModel
    {
        public int Id { get; set; }

        public DateTimeOffset AppointmentDate { get; set; }
        public int? DoctorId { get; set; }
        public int PatientId { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string PatientFullName { get; set; }
        public DateTimeOffset PatientBirthDate { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string DoctorFullName { get; set; }
        public string EmployeeFullName { get; set; }
        public string Description { get; set; }
        public int? PartnerId { get; set; }
        public int? EmployeeId { get; set; }
    }
}

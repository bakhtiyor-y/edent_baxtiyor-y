using Edent.Api.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class AppointmentManageViewModel : IViewModel
    {
        public AppointmentManageViewModel()
        {
            JointDoctors = new HashSet<int>();
        }
        public int Id { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Description { get; set; }
        public PatientManageViewModel Patient { get; set; }
        public int? DoctorId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PartnerId { get; set; }
        public int? DentalChairId { get; set; }
        public ICollection<int> JointDoctors { get; set; }
    }
}

using Edent.Api.Infrastructure.Enums;
using System;

namespace Edent.Api.ViewModels
{
    public class DoctorAppointmentViewModel : IViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public bool IsJoint { get; set; }
    }
}

using Edent.Api.Infrastructure.Enums;
using System;

namespace Edent.Api.Models
{
    public class ScheduleEventModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBusy { get; set; }
        public DateTimeOffset Starting { get; set; }
        public DateTimeOffset Last { get; set; } 
        public int AppointmentId { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

    }
}

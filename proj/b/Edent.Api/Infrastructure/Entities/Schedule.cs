using System;
using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class Schedule : Entity
    {
        public Schedule()
        {
            ScheduleSettings = new HashSet<ScheduleSetting>();
        }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<ScheduleSetting> ScheduleSettings { get; set; }
        public bool IsActive { get; set; }
        public int AdmissionDuration { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}

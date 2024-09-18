using System;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class ScheduleViewModel : IViewModel
    {
        public int DoctorId { get; set; }
        public virtual ICollection<ScheduleSettingViewModel> ScheduleSettings { get; set; }
        public bool IsActive { get; set; }
        public int AdmissionDuration { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int Id { get; set; }
    }
}

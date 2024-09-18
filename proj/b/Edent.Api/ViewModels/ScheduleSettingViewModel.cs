using System;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class ScheduleSettingViewModel : IViewModel
    {
        public int Id { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int ScheduleId { get; set; }
        public virtual ICollection<DayOfWeek> SettingDayOfWeeks { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class ScheduleSetting : Entity
    {
        public ScheduleSetting()
        {
            SettingDayOfWeeks = new HashSet<SettingDayOfWeek>();
        }
        public TimeSpan FromMinute { get; set; }
        public TimeSpan ToMinute { get; set; }
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual ICollection<SettingDayOfWeek> SettingDayOfWeeks { get; set; }
    }
}

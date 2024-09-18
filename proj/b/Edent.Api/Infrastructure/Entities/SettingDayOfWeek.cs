using System;

namespace Edent.Api.Infrastructure.Entities
{
    public class SettingDayOfWeek : Entity
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int ScheduleSettingId { get; set; }
        public virtual ScheduleSetting ScheduleSetting { get; set; }
    }
}

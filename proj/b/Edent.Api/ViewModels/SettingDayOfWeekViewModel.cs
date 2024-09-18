using System;

namespace Edent.Api.ViewModels
{
    public class SettingDayOfWeekViewModel : IViewModel
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int ScheduleSettingId { get; set; }
    }
}

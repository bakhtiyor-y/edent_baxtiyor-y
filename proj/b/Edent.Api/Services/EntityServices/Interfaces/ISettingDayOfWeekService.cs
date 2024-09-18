using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using System;
using System.Collections.Generic;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface ISettingDayOfWeekService : IEntityService<SettingDayOfWeek>
    {
        void SaveSettingDayOfWeeks(int scheduleSettingId, ICollection<DayOfWeek> settingDayOfWeeks);
    }
}

using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class SettingDayOfWeekService : EntityService<SettingDayOfWeek>, ISettingDayOfWeekService
    {
        public SettingDayOfWeekService(IRepository<SettingDayOfWeek> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }
        public void SaveSettingDayOfWeeks(int scheduleSettingId, ICollection<DayOfWeek> settingDayOfWeeks)
        {
            var settingDays = Query().AsNoTracking().Where(w => w.ScheduleSettingId == scheduleSettingId).ToList();
            var listForDelete = settingDays.Where(w => !settingDayOfWeeks.Any(a => a == w.DayOfWeek)).ToList();

            foreach (var settingDay in listForDelete)
            {
                Repository.Remove(settingDay);
            }

            foreach (var settingDay in settingDayOfWeeks)
            {
                var itemForSave = settingDays.FirstOrDefault(f => f.DayOfWeek == settingDay) ?? new SettingDayOfWeek { DayOfWeek = settingDay };
                itemForSave.ScheduleSettingId = scheduleSettingId;
                if (itemForSave.Id == 0)
                {
                    Repository.Add(itemForSave);
                }
                else
                {
                    Repository.Edit(itemForSave);
                }
            }
        }

    }
}

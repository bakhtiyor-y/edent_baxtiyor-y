using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class ScheduleSettingService : EntityService<ScheduleSetting>, IScheduleSettingService
    {
        private readonly ISettingDayOfWeekService _settingDayOfWeekService;

        public ScheduleSettingService(IRepository<ScheduleSetting> repository,
            IMapper mapper,
            ISettingDayOfWeekService settingDayOfWeekService)
            : base(repository, mapper)
        {
            _settingDayOfWeekService = settingDayOfWeekService;
        }

        public void SaveScheduleSettings(int scheduleId, ICollection<ScheduleSettingViewModel> scheduleSettings)
        {
            var settings = Query().AsNoTracking().Where(w => w.ScheduleId == scheduleId).ToList();
            var listForDelete = settings.Where(w => !scheduleSettings.Any(a => a.Id == w.Id)).ToList();

            foreach (var setting in listForDelete)
            {
                Repository.Remove(setting);
            }

            foreach (var setting in scheduleSettings)
            {
                setting.ScheduleId = scheduleId;
                var itemForSave = Mapper.Map<ScheduleSetting>(setting);
                if (setting.Id == 0)
                {
                    foreach (var settingDay in setting.SettingDayOfWeeks)
                    {
                        var weekDayForSave = new SettingDayOfWeek { DayOfWeek = settingDay };
                        itemForSave.SettingDayOfWeeks.Add(weekDayForSave);
                    }
                    Repository.Add(itemForSave);
                }
                else
                {
                    var itemForEdit = Mapper.Map<ScheduleSetting>(setting);
                    Repository.Edit(itemForEdit);
                    _settingDayOfWeekService.SaveSettingDayOfWeeks(setting.Id, setting.SettingDayOfWeeks);
                }
            }
            Repository.SaveChanges();
        }
    }
}

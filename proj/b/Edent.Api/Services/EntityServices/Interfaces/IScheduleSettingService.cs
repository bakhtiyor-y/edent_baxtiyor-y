using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.ViewModels;
using System.Collections.Generic;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IScheduleSettingService : IEntityService<ScheduleSetting>
    {
        void SaveScheduleSettings(int scheduleId, ICollection<ScheduleSettingViewModel> scheduleSettings);
    }
}

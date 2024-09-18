using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class ScheduleService : EntityService<Schedule>, IScheduleService
    {
        public ScheduleService(IRepository<Schedule> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public Schedule GetScheduleBy(int doctorId)
        {
            return Query()
                .Include(i => i.ScheduleSettings)
                    .ThenInclude(th => th.SettingDayOfWeeks)
                .FirstOrDefault(f => f.DoctorId == doctorId);
        }
    }
}

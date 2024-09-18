using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class MeasurementUnitTypeService : EntityService<MeasurementUnitType>, IMeasurementUnitTypeService
    {
        public MeasurementUnitTypeService(IRepository<MeasurementUnitType> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public override IEnumerable<MeasurementUnitType> GetAll()
        {
            return Query().Include(i => i.MeasurementUnits);
        }

        public MeasurementUnitType GetByCode(string code)
        {
            return Query()
                .Include(i => i.MeasurementUnits)
                .FirstOrDefault(f => f.Code == code);
        }
    }
}

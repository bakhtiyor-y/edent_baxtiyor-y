using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class DentalServiceService : EntityService<DentalService>, IDentalServiceService
    {
        public DentalServiceService(IRepository<DentalService> repository, IMapper mapper) : base(repository, mapper)
        {

        }

        public DentalServiceViewModel GetByIncluding(int id)
        {
            var entity = Query()
                .Include(i => i.DentalServiceGroup)
                .Include(i => i.DentalServiceCategory)
                .Include(i => i.DentalServicePrices)
                .Include(i => i.DentalServiceReceptInventorySettings)
                .FirstOrDefault(f => f.Id == id);

            return Mapper.Map<DentalServiceViewModel>(entity);
        }
    }
}

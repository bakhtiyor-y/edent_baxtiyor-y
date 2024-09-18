using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class DentalServicePriceService : EntityService<DentalServicePrice>, IDentalServicePriceService
    {
        public DentalServicePriceService(IRepository<DentalServicePrice> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}

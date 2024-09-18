using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.ViewModels;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IDentalServiceService : IEntityService<DentalService>
    {

        DentalServiceViewModel GetByIncluding(int id);
    }
}

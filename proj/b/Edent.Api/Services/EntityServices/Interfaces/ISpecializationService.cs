using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface ISpecializationService : IEntityService<Specialization>
    {
        Specialization GetByName(string name);
    }
}

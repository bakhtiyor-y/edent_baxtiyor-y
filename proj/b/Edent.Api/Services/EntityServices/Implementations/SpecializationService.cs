using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class SpecializationService : EntityService<Specialization>, ISpecializationService
    {
        public SpecializationService(IRepository<Specialization> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public Specialization GetByName(string name)
        {
            return Query().FirstOrDefault(f => f.Name.Equals(name));
        }
    }
}

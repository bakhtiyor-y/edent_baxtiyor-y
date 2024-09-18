using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class OrganizationService : EntityService<Organization>, IOrganizationService
    {
        public OrganizationService(IRepository<Organization> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public Organization GetByName(string name)
        {
            return Query().FirstOrDefault(f => f.Name.Equals(name));
        }
    }
}

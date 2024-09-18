using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class TechnicService : EntityService<Technic>, ITechnicService
    {
        public TechnicService(IRepository<Technic> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public IEnumerable<Technic> GetByName(string name)
        {

            return Query().Where(w => w.LastName.ToLower().StartsWith(name.ToLower()) || w.FirstName.ToLower().StartsWith(name.ToLower()));
        }
    }
}

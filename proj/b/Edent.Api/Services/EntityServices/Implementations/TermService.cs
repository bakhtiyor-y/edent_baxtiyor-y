using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class TermService : EntityService<Term>, ITermService
    {
        public TermService(IRepository<Term> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public Term GetByName(string name)
        {
            return Query().FirstOrDefault(f => f.Name.Equals(name));
        }
    }
}

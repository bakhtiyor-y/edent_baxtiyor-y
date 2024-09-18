using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using System.Collections.Generic;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface ITechnicService : IEntityService<Technic>
    {
        IEnumerable<Technic> GetByName(string name);
    }
}

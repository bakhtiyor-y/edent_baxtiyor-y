using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class DiagnoseService : EntityService<Diagnose>, IDiagnoseService
    {
        public DiagnoseService(IRepository<Diagnose> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }
    }
}

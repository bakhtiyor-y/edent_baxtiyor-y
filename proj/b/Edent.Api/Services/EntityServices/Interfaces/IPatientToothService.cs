using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IPatientToothService : IEntityService<PatientTooth>
    {
        bool InitPatientTeeth(int patientId, PatientAgeType type);
    }
}

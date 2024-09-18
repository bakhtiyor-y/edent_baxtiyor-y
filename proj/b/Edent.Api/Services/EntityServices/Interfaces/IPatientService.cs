using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.ViewModels;
using System.Threading.Tasks;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IPatientService : IEntityService<Patient>
    {
        PatientViewModel GetByIncluding(int id);

        PatientViewModel CreatePatient(PatientManageViewModel viewModel);
        Task<PatientViewModel> UpdatePatient(PatientManageViewModel viewModel);
    }
}

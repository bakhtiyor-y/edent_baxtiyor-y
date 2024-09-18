using System.Linq;
using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class PatientToothService : EntityService<PatientTooth>, IPatientToothService
    {
        private readonly IToothService _toothService;

        public PatientToothService(IRepository<PatientTooth> repository, IMapper mapper, IToothService toothService)
            : base(repository, mapper)
        {
            _toothService = toothService;
        }

        public bool InitPatientTeeth(int patientId, PatientAgeType type)
        {
            var toothType = type == PatientAgeType.Adult ? ToothType.Adult : ToothType.Child;
            var any = Query()
                .Include(i => i.Tooth)
                .Any(c => c.PatientId == patientId && c.Tooth.ToothType == toothType);

            if (!any)
            {
                var teeth = _toothService.Query().Where(w => w.ToothType == toothType).ToList();

                foreach (var tooth in teeth)
                {
                    Repository.Add(new PatientTooth { PatientId = patientId, ToothId = tooth.Id, ToothState = ToothState.Healthy });
                }
                return Repository.SaveChanges();
            }
            return true;
        }
    }
}

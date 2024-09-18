using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class DoctorDentalChairService : EntityService<DoctorDentalChair>, IDoctorDentalChairService
    {
        public DoctorDentalChairService(IRepository<DoctorDentalChair> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public void SaveDoctorDentalChairs(int doctorid, ICollection<int> dentalChairs)
        {
            var doctorDentalChairs = Query()
                .AsNoTracking()
                .Where(w => w.DoctorId == doctorid).ToList();

            var listForDelete = doctorDentalChairs.Where(w => !dentalChairs.Any(a => a == w.DentalChairId)).ToList();

            foreach (var doctorDentalChair in listForDelete)
            {
                Repository.Remove(doctorDentalChair);
            }

            foreach (var cId in dentalChairs)
            {
                DoctorDentalChair doctorDentalChair = new() { DentalChairId = cId, DoctorId = doctorid };
                Repository.Add(doctorDentalChair);
            }
            Repository.SaveChanges();
        }
    }
}

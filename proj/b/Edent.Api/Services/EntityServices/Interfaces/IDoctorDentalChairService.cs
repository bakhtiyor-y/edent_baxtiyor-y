using System.Collections.Generic;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IDoctorDentalChairService : IEntityService<DoctorDentalChair>
    {
        void SaveDoctorDentalChairs(int doctorid, ICollection<int> dentalChairs);
    }
}

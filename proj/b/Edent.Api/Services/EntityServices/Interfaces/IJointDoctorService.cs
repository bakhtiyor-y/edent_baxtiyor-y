using System;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IJointDoctorService : IEntityService<JointDoctor>
    {
        Appointment GetJointAppointment(int doctorId, DateTimeOffset appointmentDate);
    }
}

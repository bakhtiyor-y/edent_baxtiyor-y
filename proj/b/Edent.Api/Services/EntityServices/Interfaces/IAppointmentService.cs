using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using System;
using System.Collections.Generic;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IAppointmentService : IEntityService<Appointment>
    {
        IList<Appointment> GetAppointmentsBy(int doctorId, DateTimeOffset appointmentDate);
        IList<Appointment> GetJointAppointmentsBy(int doctorId, DateTimeOffset appointmentDate);
    }
}

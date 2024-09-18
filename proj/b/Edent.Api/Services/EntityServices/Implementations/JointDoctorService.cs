using System;
using System.Linq;
using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class JointDoctorService : EntityService<JointDoctor>, IJointDoctorService
    {
        public JointDoctorService(IRepository<JointDoctor> repository, IMapper mapper)
             : base(repository, mapper)
        {
        }

        public Appointment GetJointAppointment(int doctorId, DateTimeOffset appointmentDate)
        {
            var result = this.Query()
               .Include(i => i.Appointment)
                .ThenInclude(th => th.Patient)
               .FirstOrDefault(w => w.DoctorId == doctorId
                    && w.Appointment.AppointmentDate == appointmentDate
                    && w.Appointment.AppointmentStatus != Infrastructure.Enums.AppointmentStatus.Cancelled);

            return result?.Appointment;
        }
    }
}

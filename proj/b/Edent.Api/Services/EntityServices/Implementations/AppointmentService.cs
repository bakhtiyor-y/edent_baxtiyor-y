using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class AppointmentService : EntityService<Appointment>, IAppointmentService
    {
        public AppointmentService(IRepository<Appointment> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public IList<Appointment> GetAppointmentsBy(int doctorId, DateTimeOffset appointmentDate)
        {

            var yesterday = appointmentDate.AddDays(-1);
            var tomorrow = appointmentDate.AddDays(1);

            var result = this.Query()
                .Include(i => i.Patient)
                .Where(w => w.DoctorId == doctorId
                        && yesterday < w.AppointmentDate && tomorrow > w.AppointmentDate)
                .ToList();

            return result.Where(w => w.AppointmentDate.Date == appointmentDate.LocalDateTime.Date).ToList();
        }

        public IList<Appointment> GetJointAppointmentsBy(int doctorId, DateTimeOffset appointmentDate)
        {
            var yesterday = appointmentDate.AddDays(-1);
            var tomorrow = appointmentDate.AddDays(1);

            var result = this.Query()
                .Include(i => i.Patient)
                .Where(w => w.JointDoctors.Any(a => a.DoctorId == doctorId)
                        && yesterday < w.AppointmentDate && tomorrow > w.AppointmentDate)
                .ToList();

            return result.Where(w => w.AppointmentDate.Date == appointmentDate.LocalDateTime.Date).ToList();
        }
    }
}

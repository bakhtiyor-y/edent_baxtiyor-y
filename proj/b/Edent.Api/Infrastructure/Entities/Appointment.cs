using Edent.Api.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Appointment : Entity
    {
        public Appointment()
        {
            JointDoctors = new HashSet<JointDoctor>();
        }

        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PartnerId { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Recept Recept { get; set; }
        public virtual Partner Partner { get; set; }
        public virtual ICollection<JointDoctor> JointDoctors { get; set; }
        public int? DentalChairId { get; set; }
        public virtual DentalChair DentalChair { get; set; }
    }
}

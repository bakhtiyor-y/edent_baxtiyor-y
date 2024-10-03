using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class ReceptCustomViewModel
    {

        public ReceptCustomViewModel()
        {
            ReceptInventories = new HashSet<ReceptInventory>();
            ReceptDentalServices = new HashSet<ReceptDentalService>();
            Treatments = new HashSet<Treatment>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ReceptInventory> ReceptInventories { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
        public virtual IEnumerable<ReceptDentalService> ReceptDentalServices { get; set; } // o'zgardi
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public int? DoctorId { get; set; }
        public virtual DoctorViewModel Doctor { get; set; }
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public int? TechnicId { get; set; }
        public Technic Technic { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int? DentalChairId { get; set; }
        public virtual DentalChair DentalChair { get; set; }
        public virtual Invoice Invoice { get; set; }
        public double TechnicShare { get; set; }
        public bool IsDoctorCalculated { get; set; }
        public bool IsPartnerCalculated { get; set; }
        public bool IsTechnicCalculated { get; set; }
        public virtual DateTimeOffset CreatedDate { get; set; }
    }
}

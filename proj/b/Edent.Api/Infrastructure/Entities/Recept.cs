using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class Recept : Entity
    {
        public Recept()
        {
            ReceptInventories = new HashSet<ReceptInventory>();
            ReceptDentalServices = new HashSet<ReceptDentalService>();
            Treatments = new HashSet<Treatment>();
        }

        public string Description { get; set; }
        public virtual ICollection<ReceptInventory> ReceptInventories { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
        public virtual ICollection<ReceptDentalService> ReceptDentalServices { get; set; }
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
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
    }
}

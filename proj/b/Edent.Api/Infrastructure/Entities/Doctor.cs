using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class Doctor : Person
    {
        public Doctor()
        {
            DoctorAddresses = new HashSet<DoctorAddress>();
            Appointments = new HashSet<Appointment>();
            DoctorInTerms = new HashSet<DoctorInTerm>();
            Recepts = new HashSet<Recept>();
            DoctorDentalChairs = new HashSet<DoctorDentalChair>();
        }
        public int OrganizationId { get; set; }
        public int SpecializationId { get; set; }
        public int UserId { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<DoctorInTerm> DoctorInTerms { get; set; }
        public virtual ICollection<DoctorAddress> DoctorAddresses { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Recept> Recepts { get; set; }
        public virtual ICollection<DoctorDentalChair> DoctorDentalChairs { get; set; }

    }
}

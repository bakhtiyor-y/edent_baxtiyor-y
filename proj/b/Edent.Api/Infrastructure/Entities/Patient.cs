using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class Patient : Person
    {
        public Patient()
        {
            PatientAddresses = new HashSet<PatientAddress>();
            Appointments = new HashSet<Appointment>();
        }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double Balance { get; set; }
        public PatientAgeType PatientAgeType { get; set; }
        public virtual ICollection<PatientAddress> PatientAddresses { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<PatientTooth> PatientTooths { get; set; }
    }
}

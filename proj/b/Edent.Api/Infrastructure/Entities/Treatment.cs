using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class Treatment : Entity
    {
        public Treatment()
        {
            TreatmentDentalServices = new HashSet<TreatmentDentalService>();
        }
        public int ReceptId { get; set; }
        public virtual Recept Recept { get; set; }

        public int PatientToothId { get; set; }
        public virtual PatientTooth PatientTooth { get; set; }
        public virtual ICollection<TreatmentDentalService> TreatmentDentalServices { get; set; }
        public string Description { get; set; }
        public int? DiagnoseId { get; set; }
        public Diagnose Diagnose { get; set; }
    }
}

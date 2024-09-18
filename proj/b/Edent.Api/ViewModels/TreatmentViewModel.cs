using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class TreatmentViewModel : IViewModel
    {
        public TreatmentViewModel()
        {
            TreatmentDentalServices = new HashSet<TreatmentDentalServiceViewModel>();
        }
        public int Id { get; set; }
        public int ReceptId { get; set; }
        public string Description { get; set; }
        public virtual ReceptViewModel Recept { get; set; }
        public int PatientToothId { get; set; }
        public virtual PatientToothViewModel PatientTooth { get; set; }
        public virtual ICollection<TreatmentDentalServiceViewModel> TreatmentDentalServices { get; set; }
        public int? DiagnoseId { get; set; }
        public DiagnoseViewModel Diagnose { get; set; }
    }
}

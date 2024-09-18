using Edent.Api.ViewModels.Account;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class ReceptViewModel : IViewModel
    {
        public ReceptViewModel()
        {
            ReceptInventories = new HashSet<ReceptInventoryViewModel>();
            Treatments = new HashSet<TreatmentViewModel>();
            ReceptDentalServices = new HashSet<ReceptDentalServiceViewModel>();
        }
        public string Description { get; set; }
        public virtual ICollection<ReceptInventoryViewModel> ReceptInventories { get; set; }
        public virtual ICollection<TreatmentViewModel> Treatments { get; set; }
        public virtual ICollection<ReceptDentalServiceViewModel> ReceptDentalServices { get; set; }

        public int PatientId { get; set; }
        public virtual PatientViewModel Patient { get; set; }
        public int? DoctorId { get; set; }
        public virtual DoctorViewModel Doctor { get; set; }
        public int? EmployeeId { get; set; }
        public virtual UserViewModel Employee { get; set; }
        public int AppointmentId { get; set; }
        public virtual AppointmentViewModel Appointment { get; set; }
        public int? TechnicId { get; set; }
        public TechnicViewModel Technic { get; set; }
        public double TechnicShare { get; set; }
        public int Id { get; set; }
    }
}

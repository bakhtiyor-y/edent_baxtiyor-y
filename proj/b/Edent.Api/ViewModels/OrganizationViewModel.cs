using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class OrganizationViewModel : IViewModel
    {
        public OrganizationViewModel()
        {
            Doctors = new HashSet<DoctorViewModel>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(16)]
        public string INN { get; set; }

        [StringLength(16)]
        public string OKONX { get; set; }

        [StringLength(16)]
        public string OKED { get; set; }

        [StringLength(16)]
        public string MFO { get; set; }

        public string LogoImage { get; set; }

        public int AddressId { get; set; }
        public virtual AddressViewModel Address { get; set; }
        public virtual ICollection<DoctorViewModel> Doctors { get; set; }
        public int Id { get; set; }
    }
}

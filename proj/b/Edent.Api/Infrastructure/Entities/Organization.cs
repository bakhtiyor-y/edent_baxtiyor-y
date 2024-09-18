using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Organization : Entity
    {
        public Organization()
        {
            Doctors = new HashSet<Doctor>();
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
        public virtual Address Address { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}

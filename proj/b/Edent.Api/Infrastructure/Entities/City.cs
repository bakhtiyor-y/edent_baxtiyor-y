using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class City : Entity
    {
        public City()
        {
            Addresses = new HashSet<Address>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}

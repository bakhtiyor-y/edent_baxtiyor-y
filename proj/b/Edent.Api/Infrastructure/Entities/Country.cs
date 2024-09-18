using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Country : Entity
    {
        public Country()
        {
            Regions = new HashSet<Region>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        public virtual ICollection<Region> Regions { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Region : Entity
    {
        public Region()
        {
            Cities = new HashSet<City>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}

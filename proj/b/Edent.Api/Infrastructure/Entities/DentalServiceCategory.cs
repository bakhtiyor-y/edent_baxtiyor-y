using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class DentalServiceCategory : Entity
    {
        public DentalServiceCategory()
        {
            DentalServices = new HashSet<DentalService>();
        }

        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public virtual ICollection<DentalService> DentalServices { get; set; }
    }
}

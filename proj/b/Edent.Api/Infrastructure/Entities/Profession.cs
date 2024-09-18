using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Profession : Entity
    {
        public Profession()
        {
            Specializations = new HashSet<Specialization>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; }

    }
}

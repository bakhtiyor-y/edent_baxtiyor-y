using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Specialization : Entity
    {

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; }

        public int DisplayOrder { get; set; }
    }
}

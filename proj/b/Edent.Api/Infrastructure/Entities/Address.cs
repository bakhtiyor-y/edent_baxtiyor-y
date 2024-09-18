using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Address : Entity
    {
        [Required]
        [StringLength(512)]
        public string AddressLine1 { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class AddressViewModel : IViewModel
    {
        [Required]
        [StringLength(512)]
        public string AddressLine1 { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        public int CityId { get; set; }
        public virtual CityViewModel City { get; set; }
        public int Id { get; set; }
    }
}

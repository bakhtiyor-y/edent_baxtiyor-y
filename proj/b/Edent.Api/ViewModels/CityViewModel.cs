using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class CityViewModel : IViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }
        public int RegionId { get; set; }
        public RegionViewModel Region { get; set; }
    }
}

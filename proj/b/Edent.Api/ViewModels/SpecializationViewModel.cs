using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class SpecializationViewModel : IViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public int ProfessionId { get; set; }
        public virtual ProfessionViewModel Profession { get; set; }

        public int DisplayOrder { get; set; }
    }
}

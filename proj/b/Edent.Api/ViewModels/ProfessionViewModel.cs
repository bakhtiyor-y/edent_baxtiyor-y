using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class ProfessionViewModel : IViewModel
    {
        public ProfessionViewModel()
        {
            Specializations = new HashSet<SpecializationViewModel>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public virtual ICollection<SpecializationViewModel> Specializations { get; set; }
    }
}

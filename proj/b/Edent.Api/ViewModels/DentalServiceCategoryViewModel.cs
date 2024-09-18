using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class DentalServiceCategoryViewModel : IViewModel
    {
        public DentalServiceCategoryViewModel()
        {
            DentalServices = new HashSet<DentalServiceViewModel>();
        }
        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public virtual ICollection<DentalServiceViewModel> DentalServices { get; set; }
    }
}

using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class DentalServiceViewModel : IViewModel
    {
        public DentalServiceViewModel()
        {
            DentalServicePrices = new HashSet<DentalServicePriceViewModel>();
            ReceptInventorySettings = new HashSet<int>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
        public DentalServiceType Type { get; set; }
        public ToothState ToothState { get; set; }
        public double CurrentPrice { get; set; }
        public string CategoryName { get; set; }
        public string GroupName { get; set; }
        public int DentalServiceGroupId { get; set; }
        public int? DentalServiceCategoryId { get; set; }
        public virtual DentalServiceGroupViewModel DentalServiceGroup { get; set; }
        public virtual DentalServiceCategoryViewModel DentalServiceCategory { get; set; }
        public virtual ICollection<DentalServicePriceViewModel> DentalServicePrices { get; set; }
        public ICollection<int> ReceptInventorySettings { get; set; }
        public int Id { get; set; }
    }
}

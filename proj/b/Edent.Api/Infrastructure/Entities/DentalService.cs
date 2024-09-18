using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class DentalService : Entity
    {
        public DentalService()
        {
            ReceptDentalServices = new HashSet<ReceptDentalService>();
            DentalServicePrices = new HashSet<DentalServicePrice>();
            DentalServiceReceptInventorySettings = new HashSet<DentalServiceReceptInventorySetting>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
        public DentalServiceType Type { get; set; }
        public ToothState ToothState { get; set; }
        public int DentalServiceGroupId { get; set; }
        public int? DentalServiceCategoryId { get; set; }
        public virtual DentalServiceGroup DentalServiceGroup { get; set; }
        public virtual DentalServiceCategory DentalServiceCategory { get; set; }
        public virtual ICollection<DentalServicePrice> DentalServicePrices { get; set; }
        public virtual ICollection<DentalServiceReceptInventorySetting> DentalServiceReceptInventorySettings { get; set; }
        public virtual ICollection<TreatmentDentalService> TreatmentDentalServices { get; set; }
        public virtual ICollection<ReceptDentalService> ReceptDentalServices { get; set; }

    }
}

using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class ReceptInventorySetting : Entity
    {
        public ReceptInventorySetting()
        {
            ReceptInventorySettingItems = new HashSet<ReceptInventorySettingItem>();
            DentalServiceReceptInventorySettings = new HashSet<DentalServiceReceptInventorySetting>();
        }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<ReceptInventorySettingItem> ReceptInventorySettingItems { get; set; }
        public virtual ICollection<DentalServiceReceptInventorySetting> DentalServiceReceptInventorySettings { get; set; }
    }
}

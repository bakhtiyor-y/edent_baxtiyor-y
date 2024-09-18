using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Inventory : Entity
    {
        public Inventory()
        {
            InventoryPrices = new HashSet<InventoryPrice>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public int MeasurementUnitId { get; set; }
        public int MeasurementUnitTypeId { get; set; }
        public int Stock { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        public virtual MeasurementUnitType MeasurementUnitType { get; set; }
        public virtual ICollection<InventoryPrice> InventoryPrices { get; set; }
    }
}

using System;

namespace Edent.Api.Infrastructure.Entities
{
    public class InventoryPrice : Entity
    {
        public double Price { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}

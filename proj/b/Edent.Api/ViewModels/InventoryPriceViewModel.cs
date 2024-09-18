using System;

namespace Edent.Api.ViewModels
{
    public class InventoryPriceViewModel : IViewModel
    {
        public double Price { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public int InventoryId { get; set; }
        public virtual InventoryViewModel Inventory { get; set; }
        public int Id { get; set; }
    }
}

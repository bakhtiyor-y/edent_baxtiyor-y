namespace Edent.Api.Infrastructure.Entities
{
    public class ReceptInventorySettingItem : Entity
    {
        public ReceptInventorySettingItem()
        {
        }

        public int ReceptInventorySettingId { get; set; }
        public int Quantity { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ReceptInventorySetting ReceptInventorySetting { get; set; }
    }
}

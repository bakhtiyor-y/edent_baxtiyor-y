namespace Edent.Api.Infrastructure.Entities
{
    public class ReceptInventory : Entity
    {
        public ReceptInventory()
        {
        }

        public int ReceptId { get; set; }
        public int Quantity { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual Recept Recept { get; set; }
    }
}

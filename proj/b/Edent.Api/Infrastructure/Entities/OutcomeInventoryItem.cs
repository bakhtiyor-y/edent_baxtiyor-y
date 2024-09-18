namespace Edent.Api.Infrastructure.Entities
{
    public class OutcomeInventoryItem : Entity
    {
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public int InventoryOutcomeId { get; set; }
        public virtual InventoryOutcome InventoryOutcome { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}

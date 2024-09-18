namespace Edent.Api.Infrastructure.Entities
{
    public class IncomeInventoryItem : Entity
    {
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public int InventoryIncomeId { get; set; }
        public virtual InventoryIncome InventoryIncome { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}

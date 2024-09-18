namespace Edent.Api.ViewModels
{
    public class IncomeInventoryItemViewModel : IViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
        public int InventoryIncomeId { get; set; }
        public virtual InventoryViewModel Inventory { get; set; }
    }
}

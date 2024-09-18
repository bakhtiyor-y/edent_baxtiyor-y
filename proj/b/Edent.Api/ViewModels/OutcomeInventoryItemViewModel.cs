namespace Edent.Api.ViewModels
{
    public class OutcomeInventoryItemViewModel : IViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
        public int InventoryOutcomeId { get; set; }
        public virtual InventoryViewModel Inventory { get; set; }
    }
}

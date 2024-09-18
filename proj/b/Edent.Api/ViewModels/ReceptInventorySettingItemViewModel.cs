namespace Edent.Api.ViewModels
{
    public class ReceptInventorySettingItemViewModel : IViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int ReceptInventorySettingId { get; set; }
        public int MeasurementUnitId { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
        public InventoryViewModel SelectedInventory { get; set; }
    }
}

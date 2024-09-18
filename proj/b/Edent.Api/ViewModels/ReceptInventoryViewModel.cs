namespace Edent.Api.ViewModels
{
    public class ReceptInventoryViewModel
    {
        public ReceptInventoryViewModel()
        {
        }

        public int ReceptId { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int MeasurementUnitId { get; set; }
        public virtual InventoryViewModel Inventory { get; set; }
        public virtual ReceptViewModel Recept { get; set; }
    }
}

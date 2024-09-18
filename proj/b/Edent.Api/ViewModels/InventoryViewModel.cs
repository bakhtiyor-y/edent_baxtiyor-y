using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class InventoryViewModel : IViewModel
    {
        public InventoryViewModel()
        {
            InventoryPrices = new HashSet<InventoryPriceViewModel>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public int MeasurementUnitId { get; set; }
        public int MeasurementUnitTypeId { get; set; }
        public int Stock { get; set; }
        public string MesurementUnitCode { get; set; }
        public double CurrentPrice { get; set; }
        public virtual ICollection<InventoryPriceViewModel> InventoryPrices { get; set; }
        public virtual MeasurementUnitViewModel MeasurementUnit { get; set; }
        public virtual MeasurementUnitTypeViewModel MeasurementUnitType { get; set; }
        public int Id { get; set; }
    }
}

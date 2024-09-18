using System;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class InventoryIncomeViewModel : IViewModel
    {
        public InventoryIncomeViewModel()
        {
            InventoryItems = new HashSet<IncomeInventoryItemViewModel>();
        }
        public int Id { get; set; }
        public double TotalCost { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Who { get; set; }
        public string Description { get; set; }
        public ICollection<IncomeInventoryItemViewModel> InventoryItems { get; set; }
    }
}

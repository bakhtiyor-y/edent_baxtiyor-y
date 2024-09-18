using System;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class InventoryOutcomeViewModel : IViewModel
    {
        public InventoryOutcomeViewModel()
        {
            InventoryItems = new HashSet<OutcomeInventoryItemViewModel>();
        }
        public int Id { get; set; }
        public double TotalCost { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int? ReceptId { get; set; }
        public int? RecipientId { get; set; }
        public string Description { get; set; }
        public string Who { get; set; }
        public string Whom { get; set; }

        public ICollection<OutcomeInventoryItemViewModel> InventoryItems { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class InventoryIncome : Entity
    {
        public InventoryIncome()
        {
            InventoryItems = new HashSet<IncomeInventoryItem>();
        }
        public virtual ICollection<IncomeInventoryItem> InventoryItems { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}

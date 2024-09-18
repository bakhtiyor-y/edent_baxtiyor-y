using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class InventoryOutcome : Entity
    {
        public InventoryOutcome()
        {
            InventoryItems = new HashSet<OutcomeInventoryItem>();
        }
        public int? ReceptId { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
        public virtual Recept Recept { get; set; }
        public virtual ICollection<OutcomeInventoryItem> InventoryItems { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        public int? RecipientId { get; set; }
        public User Recipient { get; set; }
    }
}

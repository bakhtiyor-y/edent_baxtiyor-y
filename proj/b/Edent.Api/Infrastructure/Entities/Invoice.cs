using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class Invoice : Entity
    {
        public int ReceptId { get; set; }
        public double ProvidedSumm { get; set; }
        public double Discount { get; set; }
        public DiscountType DiscountType { get; set; }
        public double TotalSumm { get; set; }
        public double Debt { get; set; }
        public virtual Recept Recept { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

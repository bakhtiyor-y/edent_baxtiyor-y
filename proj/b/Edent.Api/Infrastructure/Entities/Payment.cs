using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.Infrastructure.Entities
{
    public class Payment : Entity
    {
        public int InvoiceId { get; set; }
        public double PayedSumm { get; set; }
        public virtual Invoice Invoice { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsFromBalance { get; set; }
    }
}

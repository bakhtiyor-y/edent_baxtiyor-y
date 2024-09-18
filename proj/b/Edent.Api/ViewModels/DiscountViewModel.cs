using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels
{
    public class DiscountViewModel
    {
        public int InvoiceId { get; set; }
        public double Discount { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}

using Edent.Api.Infrastructure.Enums;
using System;

namespace Edent.Api.ViewModels
{
    public class PaymentViewModel : IViewModel
    {
        public int InvoiceId { get; set; }
        public double PayedSumm { get; set; }
        public int Id { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsFromBalance { get; set; }
    }
}

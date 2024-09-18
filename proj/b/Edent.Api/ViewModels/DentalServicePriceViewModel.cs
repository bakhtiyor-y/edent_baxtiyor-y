using System;

namespace Edent.Api.ViewModels
{
    public class DentalServicePriceViewModel : IViewModel
    {
        public double Price { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public int DentalServiceId { get; set; }
        public virtual DentalServiceViewModel DentalService { get; set; }
        public int Id { get; set; }
    }
}

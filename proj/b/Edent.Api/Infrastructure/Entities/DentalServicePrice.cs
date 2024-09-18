using System;

namespace Edent.Api.Infrastructure.Entities
{
    public class DentalServicePrice : Entity
    {
        public double Price { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public int DentalServiceId { get; set; }
        public virtual DentalService DentalService { get; set; }
    }
}

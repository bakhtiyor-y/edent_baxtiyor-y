namespace Edent.Api.Infrastructure.Entities
{
    public class TreatmentDentalService : Entity
    {
        public int DentalServiceId { get; set; }
        public int TreatmentId { get; set; }
        public virtual DentalService DentalService { get; set; }
        public virtual Treatment Treatment { get; set; }
    }
}

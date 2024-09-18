namespace Edent.Api.Infrastructure.Entities
{
    public class ReceptDentalService : Entity
    {
        public int DentalServiceId { get; set; }
        public int ReceptId { get; set; }
        public virtual DentalService DentalService { get; set; }
        public virtual Recept Recept { get; set; }
    }
}

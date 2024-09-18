namespace Edent.Api.Infrastructure.Entities
{
    public class DoctorDentalChair : Entity
    {
        public DoctorDentalChair()
        {
        }

        public int DoctorId { get; set; }
        public int DentalChairId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual DentalChair DentalChair { get; set; }
    }
}

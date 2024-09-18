namespace Edent.Api.Infrastructure.Entities
{
    public class DoctorAddress : Entity
    {
        public int DoctorId { get; set; }
        public int AddressId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Address Address { get; set; }
        public bool IsActive { get; set; }
    }
}

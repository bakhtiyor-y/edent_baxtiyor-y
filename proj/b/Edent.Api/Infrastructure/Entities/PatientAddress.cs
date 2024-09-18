namespace Edent.Api.Infrastructure.Entities
{
    public class PatientAddress : Entity
    {
        public int PatientId { get; set; }
        public int AddressId { get; set; }
        public bool IsActive { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Address Address { get; set; }
    }
}

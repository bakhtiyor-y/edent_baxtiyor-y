namespace Edent.Api.Infrastructure.Entities
{
    public class DoctorInTerm : Entity
    {
        public int DoctorId { get; set; }
        public int TermId { get; set; }
        public double TermValue { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Term Term { get; set; }
    }
}

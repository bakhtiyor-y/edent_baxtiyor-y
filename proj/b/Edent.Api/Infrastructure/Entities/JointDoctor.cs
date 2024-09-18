namespace Edent.Api.Infrastructure.Entities
{
    public class JointDoctor : Entity
    {
        public int DoctorId { get; set; }

        public int AppointmentId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Appointment Appointment { get; set; }
    }
}

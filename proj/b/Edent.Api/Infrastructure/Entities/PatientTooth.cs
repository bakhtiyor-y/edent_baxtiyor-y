using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.Infrastructure.Entities
{
    public class PatientTooth : Entity
    {
        public PatientTooth()
        {
        }

        public int PatientId { get; set; }

        public int ToothId { get; set; }

        public ToothState ToothState { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Tooth Tooth { get; set; }

    }
}

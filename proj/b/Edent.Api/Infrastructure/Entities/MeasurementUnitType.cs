using System.Collections.Generic;

namespace Edent.Api.Infrastructure.Entities
{
    public class MeasurementUnitType : Entity
    {
        public MeasurementUnitType()
        {
            MeasurementUnits = new HashSet<MeasurementUnit>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<MeasurementUnit> MeasurementUnits { get; set; }
    }
}

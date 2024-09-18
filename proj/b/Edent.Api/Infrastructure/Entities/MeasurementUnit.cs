namespace Edent.Api.Infrastructure.Entities
{
    public class MeasurementUnit : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Multiplicity { get; set; }
        public bool Default { get; set; }
        public int MeasurementUnitTypeId { get; set; }
        public virtual MeasurementUnitType MeasurementUnitType { get; set; }
    }
}

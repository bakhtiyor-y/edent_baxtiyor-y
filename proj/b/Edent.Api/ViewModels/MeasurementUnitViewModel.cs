namespace Edent.Api.ViewModels
{
    public class MeasurementUnitViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Multiplicity { get; set; }
        public bool Default { get; set; }
        public int MeasurementUnitTypeId { get; set; }
        public virtual MeasurementUnitTypeViewModel MeasurementUnitType { get; set; }
        public int Id { get; set; }
    }
}

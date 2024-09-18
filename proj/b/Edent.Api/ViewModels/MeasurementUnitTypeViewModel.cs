using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class MeasurementUnitTypeViewModel : IViewModel
    {
        public MeasurementUnitTypeViewModel()
        {
            MeasurementUnits = new HashSet<MeasurementUnitViewModel>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<MeasurementUnitViewModel> MeasurementUnits { get; set; }
        public int Id { get; set; }
    }
}

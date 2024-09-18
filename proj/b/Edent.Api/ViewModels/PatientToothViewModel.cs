using Edent.Api.Infrastructure.Enums;

namespace Edent.Api.ViewModels
{
    public class PatientToothViewModel : IViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ToothId { get; set; }
        public ToothState ToothState { get; set; }
        public virtual ToothViewModel Tooth { get; set; }
    }
}

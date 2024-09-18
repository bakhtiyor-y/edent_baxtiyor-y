namespace Edent.Api.ViewModels
{
    public class TreatmentDentalServiceViewModel : IViewModel
    {
        public int Id { get; set; }
        public int TreatmentId { get; set; }
        public int DentalServiceId { get; set; }
    }
}

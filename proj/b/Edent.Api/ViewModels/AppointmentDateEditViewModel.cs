using System;

namespace Edent.Api.ViewModels
{
    public class AppointmentDateEditViewModel : IViewModel
    {
        public int Id { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public int? PartnerId { get; set; }
        public int? DentalChairId { get; set; }
    }
}

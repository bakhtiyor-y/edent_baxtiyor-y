using System;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class AppointmentDentalChairViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsBusy { get; set; }

        public string Description { get; set; }
    }
}

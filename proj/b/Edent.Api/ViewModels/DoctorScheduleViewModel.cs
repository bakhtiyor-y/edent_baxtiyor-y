using System.Collections.Generic;
using Edent.Api.Models;

namespace Edent.Api.ViewModels
{
    public class DoctorScheduleViewModel
    {
        public DoctorScheduleViewModel()
        {
            Events = new HashSet<ScheduleEventModel>();
        }

        public int DoctorId { get; set; }

        public string Name { get; set; }

        public int AdmissionDuration { get; set; }

        public ICollection<ScheduleEventModel> Events { get; set; }
    }

}

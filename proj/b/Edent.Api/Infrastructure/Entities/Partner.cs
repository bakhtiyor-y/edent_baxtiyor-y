using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Partner : Entity
    {

        public Partner()
        {
            Appointments = new HashSet<Appointment>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public double Profit { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public ProfitType ProfitType { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

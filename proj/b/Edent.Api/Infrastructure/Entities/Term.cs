using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Term : Entity
    {
        public Term()
        {
            DoctorInTerms = new HashSet<DoctorInTerm>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public TermType Type { get; set; }
        public virtual ICollection<DoctorInTerm> DoctorInTerms { get; set; }
    }
}

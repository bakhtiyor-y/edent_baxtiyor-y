using Edent.Api.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class PartnerViewModel : IViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public double Profit { get; set; }
        public ProfitType ProfitType { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }
    }
}

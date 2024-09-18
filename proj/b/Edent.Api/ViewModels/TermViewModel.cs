using Edent.Api.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class TermViewModel : IViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        public TermType Type { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class DiagnoseViewModel : IViewModel
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }
        public int Id { get; set; }
    }
}

using Edent.Api.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.ViewModels
{
    public class ToothViewModel : IViewModel
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        public int Position { get; set; }
        public ToothType ToothType { get; set; }
        public ToothDirection Direction { get; set; }
    }
}

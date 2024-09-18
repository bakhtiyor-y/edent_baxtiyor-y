using Edent.Api.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Edent.Api.Infrastructure.Entities
{
    public class Tooth : Entity
    {
        [StringLength(256)]
        public string Name { get; set; }
        public int Position { get; set; }
        public ToothType ToothType { get; set; }
        public ToothDirection Direction { get; set; }
    }
}

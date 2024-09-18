using System;

namespace Edent.Api.Infrastructure.Entities
{
    public class RefreshToken : Entity
    {
        public int UserId { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Expires { get; set; }
        public virtual User User { get; set; }
    }
}

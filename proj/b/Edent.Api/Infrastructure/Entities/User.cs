using Microsoft.AspNetCore.Identity;

namespace Edent.Api.Infrastructure.Entities
{
    public class User : IdentityUser<int>
    {
        public virtual Doctor Doctor { get; set; }
        public virtual Employee Employee { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImage { get; set; }
    }
}

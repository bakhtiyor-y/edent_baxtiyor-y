using Edent.Api.Infrastructure.Enums;
using System;

namespace Edent.Api.Infrastructure.Entities
{
    public class AuthRequest : Entity
    {
        public string UserName { get; set; }
        public string RequestToken { get; set; }
        public int Code { get; set; }
        public DateTimeOffset ExpireDate { get; set; }
        public AuthRequestType AuthRequestType { get; set; }
        public bool IsValidated { get; set; }
    }
}

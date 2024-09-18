using Edent.Api.Infrastructure.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Edent.Api.Infrastructure.Auth
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(User user, IList<string> roles, IList<Claim> claims);
    }
}

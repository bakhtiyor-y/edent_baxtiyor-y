using System.Collections.Generic;
using System.Security.Claims;

namespace Edent.Api.Infrastructure.Auth
{
    public interface IJwtTokenHandler
    {
        string WriteToken(IEnumerable<Claim> claims);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}

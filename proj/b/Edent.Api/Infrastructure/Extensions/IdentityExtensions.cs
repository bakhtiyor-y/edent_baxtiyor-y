using System;
using System.Security.Claims;

namespace Edent.Api.Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return null;

            var id = principal.FindFirst(Constants.JwtClaimIdentifiers.Id)?.Value;

            return Convert.ToInt32(id);
        }
        public static string GetUserName(this ClaimsPrincipal principal)
        {
            var userName = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userName;
        }
    }
}

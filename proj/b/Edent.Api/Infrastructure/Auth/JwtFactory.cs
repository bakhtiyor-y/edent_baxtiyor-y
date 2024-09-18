using Edent.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Edent.Api.Infrastructure.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;
        public JwtFactory(IJwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }
        public string GenerateEncodedToken(User user, IList<string> roles, IList<Claim> claims)
        {
            var identityClaims = GenerateClaimsIdentity(user.Id, user.UserName);
            identityClaims.AddClaims(roles.Select(a => new Claim(Constants.JwtClaimIdentifiers.Roles, a)));
            identityClaims.AddClaims(claims);

            return GenerateEncodedToken(identityClaims);
        }

        private ClaimsIdentity GenerateClaimsIdentity(long id, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Constants.JwtClaimIdentifiers.Id, id.ToString()),
                new Claim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.ApiAccess)
            });
        }

        private string GenerateEncodedToken(ClaimsIdentity identityClaims)
        {
            var userName = identityClaims.Name;
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
            }.Union(identityClaims.Claims);

            return _jwtTokenHandler.WriteToken(claims);
        }

        private long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

    }
}

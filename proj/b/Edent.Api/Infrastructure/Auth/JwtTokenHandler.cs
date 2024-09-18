using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Edent.Api.Infrastructure.Auth
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public JwtTokenHandler(ILogger<JwtTokenHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string WriteToken(IEnumerable<Claim> claims)
        {

            var expires = DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["Authentication:Jwt:AccessTokenExpiresIn"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Authentication:Jwt:Issuer"],
                audience: _configuration["Authentication:Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:Key"]));
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Authentication:Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = _configuration["Authentication:Jwt:Issuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                };

                var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error on get claim principal: {e}");
                return null;
            }
        }
    }
}

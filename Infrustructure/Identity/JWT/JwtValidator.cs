using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Identity.JWT
{
    public class JwtValidator
    {
        private readonly JwtSetting _jwtSetting;
        public JwtValidator(JwtSetting jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSetting.Issuer,
                    ValidAudience = _jwtSetting.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _jwtSetting.GetSymmetricSecurityKey(),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };

                var principle = tokenHandler.ValidateToken(token, validationParameters, out _);
                if (principle == null)
                {
                    throw new SecurityTokenValidationException("Token validation failed!");
                }
                return principle;
            }
            catch (Exception ex)
            {

                throw new SecurityTokenValidationException("Token validation failed!", ex);
            }
        }
    }
}

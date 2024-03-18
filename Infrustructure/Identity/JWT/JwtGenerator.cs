using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
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
    public class JwtGenerator
    {
        private readonly JwtSetting _jwtSetting;
        public JwtGenerator(JwtSetting jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }
        public string Generator(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Issuer = _jwtSetting.Issuer,
                Audience = _jwtSetting.Audience,
                Expires = DateTime.Now.AddMinutes(int.Parse(_jwtSetting.ExpirationMinutes)),
                SigningCredentials = new SigningCredentials(_jwtSetting.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

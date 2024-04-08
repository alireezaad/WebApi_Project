using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Identity.JWT
{
    public class JwtSetting
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ExpirationMinutes { get; set; }
        public string SecretKey { get; set; }
        public string RefreshTokenExpirationDays { get; set; }


        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        }
    }
}

using Application.Models.TokenModels;
using Application.Services;
using Application.Services_Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
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
        public async Task<UserToken> GeneratorAsync(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim(ClaimTypes.MobilePhone, user.Phonenumber)
                    }),
                    Issuer = _jwtSetting.Issuer,
                    Audience = _jwtSetting.Audience,
                    Expires = DateTime.Now.AddMinutes(int.Parse(_jwtSetting.ExpirationMinutes)),
                    SigningCredentials = new SigningCredentials(_jwtSetting.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor) ?? null;
                if (token == null)
                    throw new ArgumentException("Token generation failed!");

                // Write new JwtToken
                var securityToken = tokenHandler.WriteToken(token);
                if (string.IsNullOrEmpty(securityToken))
                    throw new ArgumentException("Token generation failed!");

                // Create new refresh token
                var refreshToken = Guid.NewGuid().ToString();
                
                // Add new UserToken to Database
                return new UserToken
                {
                    Token = securityToken,
                    TokenExpiration = DateTime.Now.AddMinutes(int.Parse(_jwtSetting.ExpirationMinutes)),
                    //User = user,
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = DateTime.Now.AddDays(int.Parse(_jwtSetting.RefreshTokenExpirationDays))
                };
                //return new TokenGenerateModel { Token = securityToken, RefreshToken = refreshToken };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

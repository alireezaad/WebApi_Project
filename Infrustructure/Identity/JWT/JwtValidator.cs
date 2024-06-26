﻿using Domain.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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
        private readonly IAuthenticationServices _authUser;
        private readonly ITokenServices _tokenServices;
        public JwtValidator(JwtSetting jwtSetting, IAuthenticationServices authUser, ITokenServices tokenServices)
        {
            _jwtSetting = jwtSetting;
            _authUser = authUser;
            _tokenServices = tokenServices;

        }
        public async Task<ClaimsPrincipal> ValidateToken(string token)
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
                    ClockSkew = TimeSpan.Zero
                };

                var principle = tokenHandler.ValidateToken(token, validationParameters, out _) ?? throw new SecurityTokenValidationException("Token validation failed!");

                var userPhone = (principle.FindFirst(ClaimTypes.MobilePhone)?.Value ?? null) ?? throw new ArgumentNullException("Phonenumber is null!");

                var userId = (principle.FindFirst("Id")?.Value ?? null) ?? throw new ArgumentException("User id null!");

                var user = await _authUser.FindByPhonenumberAsync(userPhone) ?? throw new ArgumentException("User not found!");

                if (!user.IsActive)
                    throw new ArgumentException("No Access!");

                return principle;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Execute(TokenValidatedContext context)
        {
            ClaimsIdentity? claimIdentity = context.Principal?.Identity as ClaimsIdentity;
            if (claimIdentity == null || !claimIdentity.Claims.Any())
            {
                context.Fail("Claims are null!");
                return;
            }

            var userId = claimIdentity.FindFirst("Id")?.Value?? null;
            if (userId == null)
            {             
                context.Fail("User id is null!");
                return;
            }

            var userPhone = claimIdentity.FindFirst(ClaimTypes.MobilePhone)?.Value ?? null;
            if (userPhone == null)
            {
                context.Fail("phonenumber is null!");
                return;
            }
            var user = await _authUser.FindByPhonenumberAsync(userPhone);
            if (!user.IsActive)
            {
                context.Fail($"{userPhone} is not active.");
                return;
            }

            if (!(context.SecurityToken is JwtSecurityToken token && _tokenServices.CheckExistsToken(user.Id, token.RawData)))
            {
                context.Fail("Unauthorized!");
                return;
            }

            context.Success();
            return;
        }
    }
}

using Application.UseCases.Managers;
using Domain.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Identity.JWT
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly JwtValidator _jwtValidator;
        public JwtMiddleware(RequestDelegate next/*, JwtValidator jwtValidator*/)
        {
            _next = next;
            //_jwtValidator = jwtValidator;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var _jwtValidator = httpContext.RequestServices.GetRequiredService<JwtValidator>();
                
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var principal = await _jwtValidator.ValidateToken(token);
                    if (principal != null)
                    {
                        httpContext.User = principal;
                        //return;
                    }
                }
                await _next(httpContext);
                //await RefreshToken(httpContext);
            }
            catch (SecurityTokenValidationException)
            {
                await RefreshToken(httpContext);
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");
                //_logger.LogError($"JwtMiddleware: {ex.Message}");
            }
        }

        private async Task RefreshToken(HttpContext httpContext)
        {
            var refreshTokn = httpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshTokn))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized: Refresh token not provided");
                return;
            }
            var _userUseCaseManager = httpContext.RequestServices.GetRequiredService<IUserUseCaseManager>();
            var accessTokens = await _userUseCaseManager.AuthUserUC.AuthenticateWithRefreshToken(refreshTokn);
            if (!accessTokens.IsSuccess)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync(accessTokens.Message);
                return;
            }
            var _jwtValidator = httpContext.RequestServices.GetRequiredService<JwtValidator>();

            var principal = await _jwtValidator.ValidateToken(accessTokens.Tokens.Token);
            if (principal == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync(accessTokens.Message);
                return;
            }

            httpContext.User = principal;
            httpContext.Response.Cookies.Append("refreshToken", accessTokens.Tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = accessTokens.RefreshTokenExpiration
            });

            await _next(httpContext);
        }
    }
}

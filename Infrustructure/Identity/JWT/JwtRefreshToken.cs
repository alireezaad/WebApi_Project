using Application.UseCases.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Identity.JWT
{
    public class JwtRefreshToken
    {
        private readonly IUserUseCaseManager _userUseCaseManager;
        private readonly JwtValidator _jwtValidator;
        public JwtRefreshToken(IUserUseCaseManager userUseCaseManager, JwtValidator jwtValidator)
        {
            _userUseCaseManager = userUseCaseManager;
            _jwtValidator = jwtValidator;

        }
        public async Task RefreshToken(AuthenticationFailedContext context)
        {
            var refreshTokn = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshTokn))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Refresh token not provided");
                return;
            }
            var accessTokens = await _userUseCaseManager.AuthUserUC.AuthenticateWithRefreshToken(refreshTokn);
            if (!accessTokens.IsSuccess)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(accessTokens.Message);
                return;
            }

            var principal = await _jwtValidator.ValidateToken(accessTokens.Tokens.Token);
            if (principal == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(accessTokens.Message);
                return;
            }

            context.HttpContext.User = principal;
            context.Success();
            context.Response.Cookies.Append("refreshToken", accessTokens.Tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = accessTokens.RefreshTokenExpiration
            });
        }
    }
}

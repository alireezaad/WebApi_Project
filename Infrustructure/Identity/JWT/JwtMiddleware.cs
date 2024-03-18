using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        //private readonly ILogger<JwtMiddleware> _logger;
        private readonly JwtValidator _jwtValidator;
        public JwtMiddleware(RequestDelegate next, JwtValidator jwtValidator)
        {
            _next = next;
            _jwtValidator = jwtValidator;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var principal = await _jwtValidator.ValidateToken(token);
                    if (principal != null)
                        httpContext.User = principal;
                }
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");
                //_logger.LogError($"JwtMiddleware: {ex.Message}");
            }
        }
    }
}

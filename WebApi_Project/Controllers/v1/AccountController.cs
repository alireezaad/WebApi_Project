using Application.Models.SmsCodeModels;
using Application.Models.UserModels;
using Application.UseCases.Managers;
using Asp.Versioning;
using Infrustructure.Identity.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_Project.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountController : ControllerBase
    {
        [GeneratedRegex("^09\\d{9}$")]
        private static partial Regex MyRegex();
        private readonly IUserUseCaseManager _userManager;
        public AccountController(IUserUseCaseManager userManager, JwtGenerator jwtGenerator)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Authenticate with Email and Password
        /// </summary>
        /// <param name="userAuthorizeModel">(Email,Password) as string</param>
        /// <returns>Jwt as string</returns>
        [HttpPost("AthenticateWithEmail&Password")]
        public IActionResult Post([FromBody] UserAuthorizeModel userAuthorizeModel)
        {
            var token = _userManager.AuthUserUC.AuthenticateWithPasswordAsync(userAuthorizeModel).Result;
            if (string.IsNullOrEmpty(token.Token) || string.IsNullOrEmpty(token.RefreshToken))
                return Unauthorized("Email or pasword is incorrect!");

            return Ok(token);
        }
        /// <summary>
        /// Authenticate with RefreshToken when Jwt was expired
        /// </summary>
        /// <param name="refreshToken">string</param>
        /// <returns></returns>
        [HttpPost("AuthenticateWithRefreshToken")]
        public IActionResult Post([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Invalid refresh token");

            var result = _userManager.AuthUserUC.AuthenticateWithRefreshToken(refreshToken).Result;
            if (!result.IsSuccess)
                return Unauthorized("Invalid refresh token");

            return Ok(result.Tokens.Token);
        }

        /// <summary>
        /// Authenticate with phonenumber
        /// </summary>
        /// <param name="phonenumber">string</param>
        /// <returns>Send a code to user's phonenumber</returns>
        [HttpGet("AuthenticateWithPhonenumber")]
        public IActionResult Get(string phonenumber)
        {
            //This validation should be implemented in UI.
            if (!MyRegex().IsMatch(phonenumber))
                return BadRequest("Phonenumber is invalid");

            _userManager.AuthUserUC.GenerateCode(phonenumber).Wait();
            return Ok(); 
        }
        /// <summary>
        /// Verify phonenumber with code 
        /// </summary>
        /// <param name="model">(phonenumber,code) as string</param>
        /// <returns>Jwt as string</returns>
        [HttpPost("VerifyPhonenumber")]
        public IActionResult Post([FromBody] VerifySmsCodeModel model)
        {
            if (string.IsNullOrEmpty(model.Phonenumber) || string.IsNullOrEmpty(model.Code))
                return BadRequest("invalid code!");

            var result = _userManager.AuthUserUC.AuthenticateWithPhonenumber(model).Result;
            if (!result.IsSuccess)
                return Unauthorized(result.Message);

            HttpContext.Response.Cookies.Append("refreshToken", result.Tokens.RefreshToken,new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = result.RefreshTokenExpiration
            });
            return Ok(result.Tokens);
        }
        /// <summary>
        /// Log out the user
        /// </summary>
        /// <returns></returns>
        [HttpGet("LogOut")]
        [Authorize]
        public IActionResult Get()
        {
            var userPhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            _userManager.AuthUserUC.LogOut(userPhone).Wait();
            return Ok();
        }
    }
}

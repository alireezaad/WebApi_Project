using Application.Models.UserModels;
using Application.UseCases.Managers;
using Asp.Versioning;
using Infrustructure.Identity.JWT;
using Microsoft.AspNetCore.Mvc;
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
        // Authorize with email & password
        [HttpPost("AthenticateWithEmail&Password")]
        public IActionResult Post([FromBody] UserAuthorizeModel userAuthorizeModel)
        {
            var token = _userManager.AuthUserUC.AuthenticateWithPasswordAsync(userAuthorizeModel).Result;
            if (string.IsNullOrEmpty(token.Token) || string.IsNullOrEmpty(token.RefreshToken))
                return Unauthorized("Email or pasword is incorrect!");

            return Ok(token);
        }

        [HttpPost("AuthenticateWithRefreshToken")]
        public IActionResult Post([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Invalid refresh token");

            return Ok(_userManager.AuthUserUC.AuthenticateWithRefreshToken(refreshToken).Result);
        }

        // Authorize with phonenumber
        [HttpGet("AuthenticateWithPhonenumber/{phonenumber}")]
        public IActionResult Get(string phonenumber)
        {
            //This validation should be implemented in UI.
            if (!MyRegex().IsMatch(phonenumber))
                return BadRequest("Phonenumber is invalid");

            _userManager.AuthUserUC.GenerateCode(phonenumber);
            return Ok(); 
        }

        [HttpPost("VerifyPhonenumber")]
        public IActionResult Post([FromBody] string phonenumber, string code)
        {
            if (string.IsNullOrEmpty(phonenumber) || string.IsNullOrEmpty(code))
                return BadRequest("invalid code!");

            var result = _userManager.AuthUserUC.AuthenticateWithPhonenumber(phonenumber, code).Result;
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


        //// GET api/<AccountController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<AccountController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<AccountController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AccountController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

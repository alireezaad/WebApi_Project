using Application.Models.UserModels;
using Application.UseCases.Managers;
using Infrustructure.Identity.JWT;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_Project.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserUseCaseManager _userManager;
        private JwtGenerator _jwtGenerator;
        public AccountController(IUserUseCaseManager userManager, JwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }
        // GET: api/<AccountController>
        [HttpPost("Athenticate")]
        public IActionResult Post([FromBody] UserAuthorizeModel userAuthorizeModel)
        {
            if (!_userManager.AuthUserUC.Authenticate(userAuthorizeModel).Result)
            {
                return Unauthorized("Email or pasword is incorrect!");
            }
            var token = _jwtGenerator.Generator(userAuthorizeModel.Email);
            return Ok(new { Token = token });
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

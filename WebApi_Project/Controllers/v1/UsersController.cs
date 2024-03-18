using Application.Models.UserModels;
using Application.UseCases.Managers;
using Application.UseCases.UserUsecases;
using Asp.Versioning;
using Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_Project.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserUseCaseManager _userManager;
        public UsersController(IUserUseCaseManager userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Get all users without their tasks
        /// </summary>
        /// <returns></returns>
        // GET: api/<UsersController>
        [HttpGet]
        public virtual async Task<IEnumerable<UserGetModel>> Get()
        {
            return await _userManager.GetAllUserUC.ExecuteAsync();
        }
        /// <summary>
        /// Get a user with tasks
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns></returns>
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public virtual async Task<UserGetModel> Get(int id)
        {
            return await _userManager.GetByIdUserUC.ExecuteAsync(id);
        }
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model">Including: Firstname,Lastname,Email and Password</param>
        /// <returns>201 StatusCode with user's info</returns>
        // POST api/<UsersController>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] UserPostModel model)
        {
            var userViewModel = await _userManager.CreateUserUC.ExecuteAsync(model);
            var url = Url.Action(nameof(Get), "Users", new { userViewModel.Id }, Request.Scheme);
            return Created(url, userViewModel);
        }
        /// <summary>
        /// Edit a user's info like firstname,lastname,email or password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>Edited user</returns>
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public virtual async Task Put(int id, [FromBody] UserPutModel model)
        {
            await _userManager.UpdateUserUC.ExecuteAsync(id, model);
        }
        /// <summary>
        /// Delete a user with id
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns></returns>
        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public virtual async Task Delete(int id)
        {
            await _userManager.DeleteUserUC.ExecuteAsync(id);
        }
    }
}

using Application.Models.LinkModels;
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
    // Deprecated(true) indicated that this version soonly going to out of service.
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserUseCaseManager _userManager;
        private readonly LinkGenerator _linkGenerator;
        public UsersController(IUserUseCaseManager userManager, LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _linkGenerator = linkGenerator;
        }
        /// <summary>
        /// Get all users without their tasks
        /// </summary>
        /// <returns></returns>
        // GET: api/<UsersController>
        [HttpGet]
        public virtual async Task<IEnumerable<UserGetModel>> Get()
        {
            var users = await _userManager.GetAllUserUC.ExecuteAsync();
            for (var i = 0; i < users.Count; i++)
            {
                var user = users[i];
                user.Links = CreateLinkforUser(user.Id);
            }
            return users;
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
            var user = await _userManager.GetByIdUserUC.ExecuteAsync(id);
            user.Links = CreateLinkforUser(user.Id);
            return user;
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

        private IEnumerable<Link> CreateLinkforUser(int id)
        {
            var links = new List<Link>()
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext,action: nameof(Get),controller: "Users",new { id },Request.Scheme), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext,action: nameof(Delete),"Users",new { id }, Request.Scheme), "delete_user", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext,nameof(Put),"Users",new { id }, Request.Scheme), "update_user", "PUT"),
            };

            return links;
        }
    }
}

using Application.Models.UserModels;
using Application.UseCases.Managers;
using Application.UseCases.UserUsecases;
using Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_Project.Controllers.v1
{
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

        // GET: api/<UsersController>
        [HttpGet]
        public virtual async Task<IEnumerable<UserGetModel>> Get()
        {
            return await _userManager.GetAllUserUC.ExecuteAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public virtual async Task<UserGetModel> Get(int id)
        {
            return await _userManager.GetByIdUserUC.ExecuteAsync(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] UserPostModel model)
        {
            var userViewModel = await _userManager.CreateUserUC.ExecuteAsync(model);
            var url = Url.Action(nameof(Get), "Users", new { userViewModel.Id }, Request.Scheme);
            return Created(url, userViewModel);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public virtual async Task Put(int id, [FromBody] UserPutModel model)
        {
            await _userManager.UpdateUserUC.ExecuteAsync(id, model);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public virtual async Task Delete(int id)
        {
            await _userManager.DeleteUserUC.ExecuteAsync(id);
        }
    }
}

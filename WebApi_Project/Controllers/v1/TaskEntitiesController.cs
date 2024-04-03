using Application.Models.TaskEntityModels;
using Application.UseCases.TaskEntityUseCases;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_Project.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskEntitiesController : ControllerBase
    {
        private readonly CreateTaskEntityUC _createTask;
        private readonly GetAllTaskEntityUC _getAllTask;
        public TaskEntitiesController(CreateTaskEntityUC createTask, GetAllTaskEntityUC getAllTask)
        {
            _createTask = createTask;
            _getAllTask = getAllTask;
        }

        /// <summary>
        /// Get all TaskEntities with all Users
        /// </summary>
        /// <returns>IEnumerable<TaskEntityGetModel></returns>
        [HttpGet]
        public async Task<IEnumerable<TaskEntityGetModel>> Get()
        {
            return await _getAllTask.ExecuteAsync();
        }
        /// <summary>
        /// Create a TaskEntity
        /// </summary>
        /// <param name="model">(Title,Description) as string, UserId as int</param>
        /// <returns>TaskEntity</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskEntityPostModel model)
        {
            var taskViewModel = await _createTask.ExecuteAsync(model);
            var url = Url.Action(nameof(Get), "TaskEntities", new { taskViewModel.Id }, Request.Scheme);
            return Created(url, taskViewModel);
        }







        // GET api/<TaskEntitiesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<TaskEntitiesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskEntitiesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

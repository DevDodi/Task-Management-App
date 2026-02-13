using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskMangementApp.DB;
using TaskMangementApp.Services;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController(ITaskService taskService)
    {
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] JsonElement taskJson)
        {
            var result = await taskService.CreateTaskAsync(taskJson);

            if (!result.Success)
                return new BadRequestObjectResult(result.Message);

            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await taskService.GetTaskAsync(id);

            if (!result.Success)
                return new BadRequestObjectResult(result.Message);

            return new OkObjectResult(result.Task);           
        }

        [HttpGet]
        public async Task<ActionResult<List<Models.Task>>> GetAllTasks()
        {
            var result = await taskService.GetAllTasksAsync();
            return new OkObjectResult(result.Tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] JsonElement taskJson)
        {            
            var result = await taskService.UpdateTaskAsync(id, taskJson);

            if (!result.Success)
                return new BadRequestObjectResult(result.Message);

            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await taskService.DeleteTaskAsync(id);

            if (!result.Success)
                return new NotFoundObjectResult(result.Message);

            return new OkResult();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagementApp.DB;
using TaskManagementApp.Services;
using TaskManagementApp.Services.Interfaces;

namespace TaskManagementApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tasks")]
    public class TaskController(ITaskService taskService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] JsonElement taskJson)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");

            var result = await taskService.CreateTaskAsync(userId, taskJson);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskManagementApp.Models.Task>> GetTask(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await taskService.GetTaskAsync(id);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkObjectResult(new { result.Task });           
        }

        [HttpGet()]
        public async Task<ActionResult<List<TaskManagementApp.Models.Task>>> GetTasks([FromQuery] Guid? projectId = null)
        {
            var result = await taskService.GetTasksAsync(projectId);
            return new OkObjectResult(new { result.Tasks });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] JsonElement taskJson)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");

            var result = await taskService.UpdateTaskAsync(userId, id, taskJson);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");

            var result = await taskService.DeleteTaskAsync(userId, id);

            if (!result.Success)
                return new NotFoundObjectResult(new { result.Message });

            return new OkResult();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tasklogs")]
    public class TaskLogController(ITaskLogService taskLogService)
    {

        [HttpGet("{taskId}")]
        [HttpGet("{taskId}/{startDateRange?}/{endDateRange?}")]
        public async Task<ActionResult<List<TaskLog>>> GetTaskLogs(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange)
        {
            if (taskId == Guid.Empty)
                return new BadRequestResult();

            var result = await taskLogService.GetTaskLogsAsync(taskId, startDateRange, endDateRange);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkObjectResult(new { result.TaskLogs });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/tasklogs")]
    public class TaskLogController(ITaskLogService taskLogService)
    {

        [HttpGet("{taskId}")]
        public async Task<ActionResult<List<TaskLog>>> GetTaskLogs(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange)
        {
            return null;
        }
    }
}

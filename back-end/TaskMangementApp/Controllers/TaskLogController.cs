using Microsoft.AspNetCore.Mvc;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/tasklogs")]
    public class TaskLogController(AppDBContext dbContext)
    {

        [HttpGet("{taskId}")]
        public HttpResponseMessage GetTaskLogs(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange)
        {
            return null;

        }
    }
}

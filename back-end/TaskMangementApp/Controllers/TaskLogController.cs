using Microsoft.AspNetCore.Mvc;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    public class TaskLogController(AppDBContext dbContext)
    {

        [HttpGet(Name = "GetTaskLogs")]
        public HttpResponseMessage GetTaskLogs(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange)
        {

        }
    }
}

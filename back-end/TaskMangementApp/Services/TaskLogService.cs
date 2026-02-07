using TaskMangementApp.Models;
using TaskMangementApp.Services.Interfaces;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services
{
    public class TaskLogService : ITaskLogService
    {
        public Task<TaskLogServiceResponse> GetTaskLogsAsync(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange)
        {
            throw new NotImplementedException();
        }
    }
}

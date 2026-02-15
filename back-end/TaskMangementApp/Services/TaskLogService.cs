using Microsoft.EntityFrameworkCore;
using TaskMangementApp.DB;
using TaskMangementApp.Services.Interfaces;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services
{
    public class TaskLogService(AppDBContext dbContext) : ITaskLogService
    {
        public Task<TaskLogServiceResponse> GetTaskLogsAsync(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange)
        {
            var startDTO = startDateRange.HasValue ? startDateRange.Value.UtcDateTime : DateTime.MinValue;
            var endDTO = endDateRange.HasValue ? endDateRange.Value.UtcDateTime : DateTime.UtcNow;

            var taskLogsQuery = dbContext.TaskLogs.Where(tl => tl.TaskId == taskId && 
            tl.LastUpdatedUtc >= startDTO && tl.LastUpdatedUtc <= endDTO);

            var taskLogs = taskLogsQuery.ToList();

            return Task.FromResult(new TaskLogServiceResponse(true, taskLogs));
        }
    }
}

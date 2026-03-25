using TaskMangementApp.Events.Interfaces;
using TaskMangementApp.Models.DTOs;
using TaskMangementApp.Models.Events;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Events
{
    public class TaskLogEventHandler : IEventHandler
    {
        private ITaskLogService taskLogService;

        public TaskLogEventHandler(ITaskLogService taskLogService)
        {
            this.taskLogService = taskLogService;
        }

        public bool CanHandle(ITaskEvent taskEvent)
        {
            switch (taskEvent)
            {
                case TaskUpdatedEvent:
                case TaskStatusChangedEvent:
                case TaskAssignedEvent:
                    return true;
                default:
                    return false;
            }
        }

        public async Task HandleAsync(ITaskEvent taskEvent)
        {
            Models.TaskLog taskLog = new Models.TaskLog
            {
                Id = Guid.NewGuid(),
                TaskId = taskEvent.TaskId,
                ChangedByUser = taskEvent.UpdatedById,
                LastUpdatedUtc = taskEvent.UpdatedAt
            };

            if (taskEvent is TaskUpdatedEvent taskUpdatedEvent)
            {
                taskLog.Action = Models.LogAction.Updated;
                taskLog.Message = $"{taskEvent.UpdatedByName} has updated the task. Title - {taskUpdatedEvent.Title}, Description - {taskUpdatedEvent.Description}";
                
            }
            else if (taskEvent is TaskAssignedEvent taskAssignedEvent)
            {

                taskLog.Action = Models.LogAction.Assigned;
                taskLog.Message = $"{taskEvent.UpdatedByName} changed assignee to {taskAssignedEvent.AssignedUser}.";
                
            }
            else if (taskEvent is TaskStatusChangedEvent taskStatusEvent)
            {
                taskLog.Action = Models.LogAction.StatusChange;
                taskLog.Message = $"{taskEvent.UpdatedByName} changed status to {taskStatusEvent.TaskState.ToString()}.";
            }

            await taskLogService.CreateTaskLogAsync(taskLog);
        }
    }
}

using TaskManagementApp.Models.DTOs;

namespace TaskManagementApp.Models.Events
{
    public abstract class TaskEventBase : ITaskEvent
    {
        public Guid TaskId { get; set; } = Guid.Empty;
        public Guid UpdatedById { get; set; } = Guid.Empty;
        public string UpdatedByEmail { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class TaskCreatedEvent : TaskEventBase
    {
        public string Title { get; set; } = string.Empty;
    }


    public class TaskUpdatedEvent : TaskEventBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class TaskStatusChangedEvent : TaskEventBase
    {
        public TaskState TaskState { get; set; }
    }

    public class TaskAssignedEvent : TaskEventBase
    {
        public Guid AssignedUser { get; set; }
        public string AssignedUserEmail { get; set; } = string.Empty;
    }
}

using System.ComponentModel;

namespace TaskMangementApp.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState Status { get; set; }
        public Guid AssignedUser { get; set; }
        public Guid AssignedProject { get; set; }
        public DateTimeOffset LastUpdatedUtc { get; set; }
    }

    public enum TaskState
    {
        [Description("Not Completed")]
        NotCompleted,

        [Description("In Progress")]
        InProgress,

        [Description("Completed")]
        Completed
    }
}

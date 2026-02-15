using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TaskMangementApp.Models
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TaskState Status { get; set; }
        public Guid AssignedUser { get; set; } = Guid.Empty;
        public Guid AssignedProject { get; set; } = Guid.Empty;

        [JsonIgnore]
        public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;
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

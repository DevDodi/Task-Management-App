using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TaskMangementApp.Models
{
    public class TaskLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public LogAction Action { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid TaskId { get; set; }
        public Guid ChangedByUser { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }

    public enum LogAction
    {
        [Description("Updated")]
        Updated,
        [Description("Created")]
        Created,
        [Description("Status Changed")]
        StatusChange,
        [Description("Assigned")]
        Assigned
    }
}

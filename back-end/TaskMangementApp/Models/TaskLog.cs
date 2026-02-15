using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TaskMangementApp.Models
{
    public class TaskLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public LogAction Action { get; set; }
        public Guid TaskId { get; set; }
        public Guid ChangedByUser { get; set; }

        [JsonIgnore]
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

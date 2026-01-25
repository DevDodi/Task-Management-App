using System.ComponentModel;

namespace TaskMangementApp.Models
{
    public class TaskLog
    {
        public Guid Id { get; set; }
        public LogAction Action { get; set; }
        public Guid TaskId { get; set; }
        public Guid ChangedByUser { get; set; }
        public DateTimeOffset LastUpdatedUtc { get; set; }
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

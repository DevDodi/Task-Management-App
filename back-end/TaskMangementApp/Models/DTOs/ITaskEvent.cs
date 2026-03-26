namespace TaskMangementApp.Models.DTOs
{
    public interface ITaskEvent
    {
        Guid TaskId { get; set; }
        Guid UpdatedById { get; set; }
        string UpdatedByEmail { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}

namespace TaskMangementApp.Models.DTOs
{
    public interface ITaskEvent
    {
        Guid TaskId { get; set; }
        Guid UpdatedById { get; set; }
        string UpdatedByName { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}

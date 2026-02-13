namespace TaskMangementApp.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public Guid OwnerId { get; set; } = Guid.Empty;
    }
}

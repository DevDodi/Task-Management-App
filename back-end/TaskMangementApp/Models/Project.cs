namespace TaskMangementApp.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string? Description { get; set; }
        private Guid _ownerId {  get; set; } = Guid.Empty;
        public Guid OwnerId
        {
            get => _ownerId;
            set
            {
                Status = value == Guid.Empty ? ProjectStatus.Unassigned : ProjectStatus.Assigned;
                _ownerId = value;
            }
        }
        public ProjectStatus Status { get; set; } = ProjectStatus.Unassigned;
        public enum ProjectStatus
        {
            Unassigned,
            Assigned
        }
    }
}

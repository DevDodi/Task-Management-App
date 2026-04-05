namespace TaskManagementApp.Models.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Email { get; set; }
    }
}

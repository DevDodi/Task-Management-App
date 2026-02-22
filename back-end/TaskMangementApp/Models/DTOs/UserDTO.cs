namespace TaskMangementApp.Models.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

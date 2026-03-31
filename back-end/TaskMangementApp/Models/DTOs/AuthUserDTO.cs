namespace TaskMangementApp.Models.DTOs
{
    public class AuthUserDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

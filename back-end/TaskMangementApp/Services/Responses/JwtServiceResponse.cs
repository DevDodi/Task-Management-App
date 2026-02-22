using TaskMangementApp.Models;

namespace TaskMangementApp.Services.Responses
{
    public class JwtServiceResponse : BaseServiceResponse
    {
        public User? User { get; set; }
        public string? AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }

        public JwtServiceResponse(bool success, string? message = null) : base(success, message)
        {

        }

        public JwtServiceResponse(bool success, User user, string accessToken, DateTime expiresIn) : base(success)
        {
            User = user;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}

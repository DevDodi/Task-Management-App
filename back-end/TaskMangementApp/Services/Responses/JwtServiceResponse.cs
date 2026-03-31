using TaskMangementApp.Models;

namespace TaskMangementApp.Services.Responses
{
    public class JwtServiceResponse : BaseServiceResponse
    {
        public Guid UserId { get; set; }
        public string? AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }

        public JwtServiceResponse(bool success, string? message = null) : base(success, message)
        {

        }

        public JwtServiceResponse(bool success, Guid userId, string accessToken, DateTime expiresIn) : base(success)
        {
            UserId = userId;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }
    }
}

using TaskMangementApp.Models;

namespace TaskMangementApp.Services.Responses
{
    public class UserServiceResponse : BaseServiceResponse
    {
        public User? User { get; set; }

        public UserServiceResponse(bool success, User? user = null, string ? message = null) : base(success, message)
        {
            User = user;
        }
    }
}

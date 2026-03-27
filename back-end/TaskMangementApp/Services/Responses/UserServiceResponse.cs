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

    public class UserServiceResponseList : BaseServiceResponse
    {
        public List<User?> Users { get; set; }

        public UserServiceResponseList(bool success, List<User?> users, string? message = null) : base(success, message)
        {
            Users = users;
        }
    }
}

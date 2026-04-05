using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Models;

namespace TaskManagementApp.Services.Responses
{
    public class UserServiceResponse : BaseServiceResponse
    {
        public UserDTO? User { get; set; }

        public UserServiceResponse(bool success, UserDTO? user = null, string ? message = null) : base(success, message)
        {
            User = user;
        }
    }

    public class UserServiceResponseList : BaseServiceResponse
    {
        public List<UserDTO?> Users { get; set; }

        public UserServiceResponseList(bool success, List<UserDTO?> users, string? message = null) : base(success, message)
        {
            Users = users;
        }
    }
}

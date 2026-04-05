using System.Net;
using System.Text.Json;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.DB;
using TaskManagementApp.Models;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;

namespace TaskManagementApp.Services
{
    public class UserService(AppDBContext dbContext) : IUserService
    {
        public Task<UserServiceResponse> CreateUserAsync(JsonElement userJson)
        {
            AuthUserDTO? userDTO = null;
            try { userDTO = JsonSerializer.Deserialize<AuthUserDTO>(userJson); } catch { }

            if (userDTO is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            if (dbContext.Users.Any(u => u.Email == userDTO.Email))
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "This email is currently being used by another User."));
        
            var user = new User
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }

        public Task<UserServiceResponse> DeleteUserAsync(Guid id)
        {
            var existingUser = dbContext.Users.Find(id);

            if (existingUser is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "User does not exist"));

            dbContext.Users.Remove(existingUser);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }
        public Task<UserServiceResponseList> GetUsersAsync()
        {
            var users = dbContext.Users.Select(u => new UserDTO { Id = u.Id, Email = u.Email }).ToList();
            return System.Threading.Tasks.Task.FromResult(new UserServiceResponseList(true, users));
        }

        public Task<UserServiceResponse> GetUserAsync(Guid id)
        {
            var user = dbContext.Users.Find(id);

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            var userDTO = new UserDTO { Id = user.Id, Email = user.Email };

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true, userDTO));
        }

        public Task<UserServiceResponse> UpdateUserAsync(Guid id, JsonElement userJson)
        {
            AuthUserDTO? userDTO = null;
            try { userDTO = JsonSerializer.Deserialize<AuthUserDTO>(userJson); } catch { }

            if (userDTO is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            if (dbContext.Users.Any(u => u.Email == userDTO.Email && u.Id != userDTO.Id))
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "This email is currently being used by another User."));

            var existingUser = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            var user = new User
            {
                Id = id,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            };

            dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }
    }
}

using System.Net;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services.Interfaces;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services
{
    public class UserService(AppDBContext dbContext) : IUserService
    {
        public Task<UserServiceResponse> CreateUserAsync(string userJson)
        {
            User? user = JsonSerializer.Deserialize<User>(userJson);

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            if (dbContext.Users.Any(u => u.Email == user.Email))
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "This email is currently being used by another User."));
        
            user.Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id;

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }

        public Task<UserServiceResponse> DeleteUserAsync(Guid id)
        {
            var user = new User { Id = id };
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }

        public Task<UserServiceResponse> GetUserAsync(Guid id)
        {
            var user = dbContext.Users.Find(id);

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true, user));
        }

        public Task<UserServiceResponse> UpdateUserAsync(Guid id, string userJson)
        {
            User? user = JsonSerializer.Deserialize<User>(userJson);

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            if (dbContext.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "This email is currently being used by another User."));

            var existingUser = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            existingUser = user;
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }
    }
}

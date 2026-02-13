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
        public Task<UserServiceResponse> CreateUserAsync(JsonElement userJson)
        {
            User? user = null;
            try { user = JsonSerializer.Deserialize<User>(userJson); } catch { }

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            if (dbContext.Users.Any(u => u.Email == user.Email))
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "This email is currently being used by another User."));
        
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

        public Task<UserServiceResponse> GetUserAsync(Guid id)
        {
            var user = dbContext.Users.Find(id);

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true, user));
        }

        public Task<UserServiceResponse> UpdateUserAsync(Guid id, JsonElement userJson)
        {
            User? user = null;
            try { user = JsonSerializer.Deserialize<User>(userJson); } catch { }

            if (user is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            if (dbContext.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false, null, "This email is currently being used by another User."));

            var existingUser = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser is null)
                return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(false));

            user.Id = id;
            dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new UserServiceResponse(true));
        }
    }
}

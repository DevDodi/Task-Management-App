using System.Text.Json;
using TaskMangementApp.Models;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserServiceResponse> CreateUserAsync(JsonElement userJson);
        Task<UserServiceResponseList> GetUsersAsync();
        Task<UserServiceResponse> GetUserAsync(Guid id);
        Task<UserServiceResponse> UpdateUserAsync(Guid id, JsonElement userJson);
        Task<UserServiceResponse> DeleteUserAsync(Guid id);
    }
}

using System.Text.Json;
using TaskManagementApp.Models;
using TaskManagementApp.Services.Responses;

namespace TaskManagementApp.Services.Interfaces
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

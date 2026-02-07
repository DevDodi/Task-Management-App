using TaskMangementApp.Models;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserServiceResponse> CreateUserAsync(string userJson);
        Task<UserServiceResponse> GetUserAsync(Guid id);
        Task<UserServiceResponse> UpdateUserAsync(Guid id, string userJson);
        Task<UserServiceResponse> DeleteUserAsync(Guid id);
    }
}

using System.Text.Json;
using TaskManagementApp.Services.Responses;

namespace TaskManagementApp.Services.Interfaces
{
    public interface IJwtService
    {
        Task<JwtServiceResponse> Authenticate(JsonElement userJson);
    }
}

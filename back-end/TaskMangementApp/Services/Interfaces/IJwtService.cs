using System.Text.Json;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface IJwtService
    {
        Task<JwtServiceResponse> Authenticate(JsonElement userJson);
    }
}

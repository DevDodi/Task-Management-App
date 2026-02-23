using System.Text.Json;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface IJwtSevice
    {
        Task<JwtServiceResponse> Authenticate(JsonElement userJson);
    }
}

using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Models.DTOs;
using TaskMangementApp.Services.Interfaces;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services
{
    public class JwtService(AppDBContext dbContext, IConfiguration config) : IJwtSevice
    {
        public Task<JwtServiceResponse> Authenticate(JsonElement userJson)
        {
            UserDTO? userDTO = null;
            try { userDTO = JsonSerializer.Deserialize<UserDTO>(userJson); } catch { }

            if (userDTO is null)
                return System.Threading.Tasks.Task.FromResult(new JwtServiceResponse(false));

            var matchingUser = dbContext.Users.FirstOrDefault(u => u.Email == userDTO.Email);
            if (matchingUser == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, matchingUser.PasswordHash))
                return System.Threading.Tasks.Task.FromResult(new JwtServiceResponse(false, "Invalid credentials"));

            var jwt = config.GetSection("JwtSettings");
            var issuer = jwt["Issuer"];
            var audience = jwt["Audience"];

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!));

            var expiresIn = DateTime.UtcNow.AddMinutes(
                double.Parse(jwt["ExpiryMinutes"]!));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, matchingUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, matchingUser.Email),
                }),
                Expires = expiresIn,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return System.Threading.Tasks.Task.FromResult(new JwtServiceResponse(true, matchingUser.Id, accessToken, expiresIn));
        }
    }
}

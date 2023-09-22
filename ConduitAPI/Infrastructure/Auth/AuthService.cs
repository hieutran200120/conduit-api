
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConduitAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ConduitAPI.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        public const int ACCESS_TOKEN_LIFE_TIME = 60;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string plainPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashPassword);
        }

        public string GenerateToken(User user)
        {
            var credential = _configuration["AppCredential"];
            var key = Encoding.ASCII.GetBytes(credential);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                    new Claim(ClaimTypes.Role, "admin")
                }),
                Expires = DateTime.UtcNow.AddMinutes(ACCESS_TOKEN_LIFE_TIME),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}

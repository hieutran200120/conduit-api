using ConduitAPI.Entities;

namespace ConduitAPI.Infrastructure.Auth
{
    public interface IAuthService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string plainPassword, string hashPassword);
        public string GenerateToken(User user);
    }
}

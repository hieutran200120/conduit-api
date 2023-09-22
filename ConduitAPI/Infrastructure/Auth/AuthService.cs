
namespace ConduitAPI.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string plainPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashPassword);
        }
    }
}

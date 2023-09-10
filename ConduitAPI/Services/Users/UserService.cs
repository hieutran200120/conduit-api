using ConduitAPI.Entities;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI.Services.Users
{
    public class UserService:IUserService
    {
        private readonly MainDbContext _mainDbContext;
        public UserService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }
        public async Task<UserDto> GetCurrentUser(string username)
        {
            var user = await _mainDbContext.Users.AsNoTracking().Where(x => x.Username == username)
                .Select(x => new UserDto
                {
                    Bio = x.Bio,
                    Email = x.Email,
                    Image = "", //TODO: Update image for entity user later
                    Username = x.Username,
                    Token = "" //TODO: Update toke with authen later

                })
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            return user;
        }
        public async Task<UserDto> Register(RegisterRequestDto request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };
            _mainDbContext.Users.Add(user);
            await _mainDbContext.SaveChangesAsync();
            return new UserDto
            {
                Bio = user.Bio,
                Email = user.Email,
                Image = "", //TODO: Update image for entity user later
                Username = user.Username,
                Token = "" //TODO: Update toke with authen later
            };
        }
    }
}

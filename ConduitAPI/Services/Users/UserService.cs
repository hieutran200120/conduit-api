using ConduitAPI.Entities;
using ConduitAPI.Infrastructure.Auth;
using ConduitAPI.Infrastructure.Exceptions;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        private readonly ICurrentUser _currentUser;
        public UserService(MainDbContext mainDbContext, IAuthService authService, ICurrentUser currentUser) 
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
            _currentUser = currentUser;
        }

        public async Task<UserDto> GetCurrentUser()
        {

            var user = await _mainDbContext.Users.AsNoTracking().Where(x => x.Id == _currentUser.Id)
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

        public async Task<UserDto> UpdateCurrentUser(string username, UpdateCurrentUserRequestDto request)
        {
            var user = await _mainDbContext.Users.Where(x => x.Username == username).FirstOrDefaultAsync(); //luu 1 ban sao trong change tracker, còn 1 bản là e đang để code
            if (user == null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound ,"User does not exist");
            }
            user.Bio = request.Bio;
            user.Email = request.Email;
            user.Username = request.Username;
            //TODO: update image
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

        public async Task<UserDto> Register(RegisterRequestDto request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = _authService.HashPassword(request.Password),
            };
            _mainDbContext.Users.Add(user);
            await _mainDbContext.SaveChangesAsync();
            return new UserDto
            {
                Bio = user.Bio,
                Email = user.Email,
                Image = "", //TODO: Update image for entity user later
                Username = user.Username,
                Token = _authService.GenerateToken(user)
            };
        }
    }
}

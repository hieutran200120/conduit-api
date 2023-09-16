using ConduitAPI.Entities;
using ConduitAPI.Service.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI.Service.Users
{
    public class UserService : IUserService 
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
                    Image = x.Image, 
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
                throw new Exception("User does not exist");
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
                Image = user.Image,
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
                Password = request.Password
            };
            _mainDbContext.Users.Add(user);
            await _mainDbContext.SaveChangesAsync();
            return new UserDto
            {
                Bio = user.Bio,
                Email = user.Email,
                Image = user.Image,
                Username = user.Username,
                Token = "" //TODO: Update toke with authen later
            };
        }
        public async Task<List<UserDto>> GetAllUser()
        {
            var toDo = await _mainDbContext.Users.AsNoTracking()
                .Select(x => new UserDto
                {
                    Username = x.Username,
                    Email = x.Email
                })
                .ToListAsync();

            if (toDo == null)
            {
                throw new Exception("ToDo does not exist");
            }

            return toDo;
        }
        public async Task<UserDto> DeleteUser(string username)
        {
            var user = await _mainDbContext.Users.AsNoTracking().Where(x => x.Username == username).FirstOrDefaultAsync();
            if(user == null)
            {
                throw new Exception("User does not exist");
            }    
            _mainDbContext.Users.Remove(user);
            await _mainDbContext.SaveChangesAsync();
            return new UserDto
            {
               Username=user.Username,
               Email=user.Email,
               Image=user.Image,
               Bio = user.Bio,
            };
        }
    }
}

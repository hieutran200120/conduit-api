using ConduitAPI.Entities;
using ConduitAPI.Infrastructure.Auth;
using ConduitAPI.Infrastructure.Exceptions;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ConduitAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        private readonly ICurrentUser _currentUser;
        private readonly IWebHostEnvironment _hostEnvironment;
        public UserService(MainDbContext mainDbContext, IAuthService authService, ICurrentUser currentUser, IWebHostEnvironment hostEnvironment) 
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
            _currentUser = currentUser;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<UserDto> GetCurrentUser()
        {

            var user = await _mainDbContext.Users.AsNoTracking().Where(x => x.Id == _currentUser.Id)
                .Select(x => new UserDto
                {
                    Bio = x.Bio,
                    Email = x.Email,
                    Image = x.Image,
                    Username = x.Username,
                    Token = _authService.GenerateToken(x),

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
            
            await _mainDbContext.SaveChangesAsync();
            return new UserDto
            {
                Bio = user.Bio,
                Email = user.Email,
                Image = await SaveImage(request.ImageFile),
                Username = user.Username,
                Token = _authService.GenerateToken(user)
            };
        }

        public async Task<UserDto> Register(RegisterRequestDto request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = _authService.HashPassword(request.Password),
                Image = await SaveImage(request.ImageFile)
        };
          /*  user.Image = await SaveImage(request.ImageFile);*/
            _mainDbContext.Users.Add(user);
            await _mainDbContext.SaveChangesAsync();
            return new UserDto
            {
                Bio = user.Bio,
                Email = user.Email,
                Image =user.Image, 
                Username = user.Username,
                Token = _authService.GenerateToken(user)
            };
        }
        public async Task<UserDto> Login(LoginRequestDto Request)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(a => a.Email == Request.Email);
            if (user == null)
            {
                throw new Exception("Không có email này");
            }
            bool checkPassword = _authService.VerifyPassword(Request.Password, user.Password);
            if (!checkPassword)
            {
                throw new Exception("mật khẩu không hợp lệ");
            }
            return new UserDto 
            {
                Bio = user.Bio,
                Email = user.Email,
                Image = user.Image,
                Username = user.Username,
                Token = _authService.GenerateToken(user) 
            };
        }
        /*[NonAction]*/
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }
}

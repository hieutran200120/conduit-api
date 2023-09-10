using ConduitAPI.Services.Users.Dtos;

namespace ConduitAPI.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterRequestDto request);
        Task<UserDto> GetCurrentUser(string username);
    }
}

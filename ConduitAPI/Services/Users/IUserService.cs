using ConduitAPI.Services.Users.Dtos;

namespace ConduitAPI.Services.Users
{
    public interface IUserService
    {
        //Task<> Login();
        Task<UserDto> Register(RegisterRequestDto request);
        Task<UserDto> GetCurrentUser();

        //TODO: remove username when implement authenticate
        Task<UserDto> UpdateCurrentUser(string username, UpdateCurrentUserRequestDto request);
    }
}

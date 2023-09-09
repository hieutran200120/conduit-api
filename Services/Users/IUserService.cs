using ConduitAPI.Services.Users.Dtos;

namespace ConduitAPI.Services.Users
{
    public interface IUserService
    {
        //Task<> Login();
        Task<UserDto> Register(RegisterRequestDto request);
        //TODO: remove username when implement authenticate
        Task<UserDto> GetCurrentUser(string username);

        //TODO: remove username when implement authenticate
        Task<UserDto> UpdateCurrentUser(string username, UpdateCurrentUserRequestDto request);
    }
}

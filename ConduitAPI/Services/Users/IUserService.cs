﻿using ConduitAPI.Services.Users.Dtos;

namespace ConduitAPI.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> Login(LoginRequestDto Request);
        Task<UserDto> Register(RegisterRequestDto request);
        Task<UserDto> GetCurrentUser();

        //TODO: remove username when implement authenticate
        Task<UserDto> UpdateCurrentUser(string username, UpdateCurrentUserRequestDto request);
        Task<string> SaveImage(IFormFile imageFile);
    }
}

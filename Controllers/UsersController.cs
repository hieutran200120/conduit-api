﻿using ConduitAPI.Services.Users;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ConduitAPI.Controllers
{
    [Route("api/[controller]")] //repository, mediator, cqrs, DI, // S.O.L.I.D : S-> Single Responsibilty
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok("Login success");
        }

        //TODO: remove username when implement authenticate
        [HttpGet("{username}")]
        public async Task<IActionResult> GetCurrentUser(string username)
        {
            var res = await _userService.GetCurrentUser(username);
            return Ok(res);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateCurrentUser(string username, UpdateCurrentUserRequestDto request)
        {
            var res = await _userService.UpdateCurrentUser(username, request);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var res = await _userService.Register(request);
            return Ok(res);
        }
    }
}


using ConduitAPI.Services.Users;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConduitAPI.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto Request)
        {
            var res = await _userService.Login(Request);
            return Ok(res);
        }

        //TODO: remove username when implement authenticate
        [Authorize(Roles = "user, admin")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var res = await _userService.GetCurrentUser();
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

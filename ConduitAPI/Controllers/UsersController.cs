using ConduitAPI.Services.Users;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.AspNetCore.Http;
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
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var res = await _userService.Register(request);
            return Ok(res);
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> GetCurrentUser(string username)
        {
            var res = await _userService.GetCurrentUser(username);
            return Ok(res);
        }
    }
}

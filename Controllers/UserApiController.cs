using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register - Used by frontend for user registration
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/user/login - Used by frontend for user login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.LoginAsync(loginDto);
                
                // Set session data after successful login
                HttpContext.Session.SetString("Username", result.User.Username);
                HttpContext.Session.SetInt32("UserId", result.User.Id);
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

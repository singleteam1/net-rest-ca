using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Auth;
using Shared.Abstractions.Services;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        private readonly IAuthService _service = service;

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            string email = this.User.Claims.FirstOrDefault().Value;
            return Ok(await _service.Me(email));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDto dto)
        {
            return Ok(await _service.Register(dto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto dto)
        {
            return Ok(await _service.Login(dto));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            return Ok(await _service.RefreshToken(dto));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LoginUserAsync()
        {
            string email = this.User.Claims.FirstOrDefault().Value;
            return Ok(await _service.Logout(email));
        }
    }
}

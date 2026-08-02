using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Services;
using Shared.Dtos.User;

namespace Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto dto)
        {
            string email = this.User.Claims.FirstOrDefault().Value;
            return Ok(await _userService.UpdateUser(email, dto));
        }
    }
}

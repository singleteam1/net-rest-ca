using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Services;
using Shared.Dtos.Role;

namespace Api.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController(IRoleService service) : ControllerBase
    {
        private readonly IRoleService _service = service;

        [HttpGet("my-roles")]
        [Authorize]
        public async Task<IActionResult> MyRoles()
        {
            string email = this.User.Claims.FirstOrDefault().Value;
            return Ok(await _service.MyRoles(email));
        }

        [HttpGet("role-list")]
        [Authorize(Roles = "Admin")]
        public IActionResult AllRoles()
        {
            return Ok(_service.AllRoles());
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto dto)
        {
            return Ok(await _service.CreateRoleAsync(dto));
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] AssignRoleDto dto)
        {
            return Ok(await _service.AssignRoleAsync(dto.roleName, dto.email));
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleDto dto)
        {
            return Ok(await _service.DeleteRoleAsync(dto));
        }
    }
}

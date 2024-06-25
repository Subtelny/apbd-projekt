using APBD_PROJEKT.controllers.Request;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var result = await roleService.CreateRoleAsync(roleName);
        if (result)
        {
            return Ok(new { Message = "Role created successfully" });
        }
        return BadRequest(new { Message = "Role creation failed" });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
    {
        var result = await roleService.AssignRoleToUserAsync(model.Username, model.RoleName);
        if (result)
        {
            return Ok(new { Message = "Role assigned successfully" });
        }
        return BadRequest(new { Message = "Role assignment failed" });
    }
}
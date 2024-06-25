using Microsoft.AspNetCore.Identity;

namespace APBD_PROJEKT.Service;

public class RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    : IRoleService
{
    public async Task<bool> CreateRoleAsync(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return false;
        }
        var result = await roleManager.CreateAsync(new IdentityRole(roleName));
        return result.Succeeded;

    }

    public async Task<bool> AssignRoleToUserAsync(string username, string roleName)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user == null || !await roleManager.RoleExistsAsync(roleName))
        {
            return false;
        }
        var result = await userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;

    }
}
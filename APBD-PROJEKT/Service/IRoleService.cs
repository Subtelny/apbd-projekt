namespace APBD_PROJEKT.Service;

public interface IRoleService
{
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> AssignRoleToUserAsync(string username, string roleName);
}
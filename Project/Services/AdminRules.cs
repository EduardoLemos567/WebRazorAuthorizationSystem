using Microsoft.AspNetCore.Identity;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Services;

public class AdminRules
{
    private readonly UserManager<Models.Identity> users;
    private readonly RoleManager<Models.Role> roles;
    public AdminRules(UserManager<Models.Identity> users, RoleManager<Models.Role> roles)
    {
        this.users = users;
        this.roles = roles;
    }
    public IQueryable<Models.Role> GetAllNonDefaultRoles()
    {
        return from role in roles.Roles
               where role.Name != DefaultRoles.User
                  && role.Name != DefaultRoles.Staff
                  && role.Name != DefaultRoles.Admin
               select role;
    }
    public IEnumerable<string> GetUserNonDefaultRoles(Models.Identity user)
    {
        return from role in users.GetRolesAsync(user).Result
               where !DefaultRoles.IsDefaultRole(role)
               select role;
    }
    public Task<Models.Identity?> TryFindUserAsync(int? id)
    {
        if (id is null) { return Task.FromResult<Models.Identity?>(null); }
        return users.FindByIdAsync(id.ToString()!);
    }
    public Task<Models.Role?> TryFindRoleAsync(int? id)
    {
        if (id is null) { return Task.FromResult<Models.Role?>(null); }
        return roles.FindByIdAsync(id.ToString()!);
    }
    public async Task<Claim?> TryGetPermissionClaimAsync(Models.Identity user)
    {
        return (await users.GetClaimsAsync(user))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
    }
    public async Task<Claim?> TryGetPermissionClaimAsync(Models.Role role)
    {
        return (await roles.GetClaimsAsync(role))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
    }
    public Task<bool> CanModifyUserPermissionsAsync(Models.Identity user)
    {
        return users.IsInRoleAsync(user, DefaultRoles.Staff);
    }
    public bool CanModifyRolePermissions(string roleName)
    {
        return !DefaultRoles.IsDefaultRole(roleName);
    }
    public Task<bool> CanModifyUserRolesAsync(Models.Identity user)
    {
        return CanModifyUserPermissionsAsync(user);
    }
    public bool CanCRUDRole(string roleName)
    {
        return !DefaultRoles.IsDefaultRole(roleName);
    }
}

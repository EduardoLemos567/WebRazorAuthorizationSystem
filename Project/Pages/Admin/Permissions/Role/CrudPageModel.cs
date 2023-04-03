using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Role;

public class CrudPageModel : PageModel
{
    protected readonly RoleManager<Models.Role> roles;
    protected readonly CachedDefaultData cachedData;
    public CrudPageModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData)
    {
        this.roles = roles;
        this.cachedData = cachedData;
    }    
    public IReadOnlyList<string> Permissions => cachedData.SortedPermissionsStrings;
    protected async Task<Models.Role?> TryFindRoleAsync(int? id)
    {
        if (id is null) { return null; }
        return await roles.FindByIdAsync(id.ToString()!);
    }
    public static bool CanModifyPermissions(Models.Role role)
    {
        return role.Name != DefaultRoles.User.ToString() && role.Name != DefaultRoles.Admin.ToString();
    }
    protected IActionResult NotAllowedModify() => Content("Role 'User' cannot have permissions.");
    protected async Task<Claim?> TryFindOldClaimAsync(Models.Role role)
    {
        return (await roles.GetClaimsAsync(role))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
    }    
}

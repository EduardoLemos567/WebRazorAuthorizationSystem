using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Identity;

public class CrudPageModel : PageModel
{
    protected readonly UserManager<Models.Identity> users;
    protected readonly CachedDefaultData cachedData;
    public CrudPageModel(UserManager<Models.Identity> users, CachedDefaultData cachedData)
    {
        this.users = users;
        this.cachedData = cachedData;
    }
    public IReadOnlyList<string> Permissions => cachedData.SortedPermissionsStrings;
    protected async Task<Models.Identity?> TryFindUserAsync(int? id)
    {
        if (id is null) { return null; }
        return await users.FindByIdAsync(id.ToString()!);
    }
    protected Task<bool> CanModifyPermissionsAsync(Models.Identity user)
    {
        return users.IsInRoleAsync(user, DefaultRoles.Staff.ToString());
    }
    protected IActionResult NotAllowedModify() => Content("User is not member of 'Staff', cant have permissions.");
    protected async Task<Claim?> TryFindOldClaimAsync(Models.Identity user)
    {
        return (await users.GetClaimsAsync(user))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
    }
}

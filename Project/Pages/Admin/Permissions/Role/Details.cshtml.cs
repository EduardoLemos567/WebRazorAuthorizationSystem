using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Permissions.Role;

public class DetailsModel : CrudPageModel
{
    public DetailsModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public IReadOnlyList<string> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!CanModifyPermissions(role)) { return NotAllowedModify(); }
        Role = Models.SummaryRole.FromRole(role);
        var oldClaim = await TryFindOldClaimAsync(role);
        if (oldClaim is not null && oldClaim.Value.Length > 0)
        {
            SelectedPermissions = (from p in Requirements.PermissionsFromString(oldClaim.Value) select p.ToString()).ToArray();
        }
        else
        {
            SelectedPermissions = Array.Empty<string>();
        }
        return Page();
    }
}

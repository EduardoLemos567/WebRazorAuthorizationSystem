using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Permissions.Role;

[RequirePermission(Places.PermissionsRole, Actions.Read)]
public class DetailsModel : CrudPageModel
{
    public DetailsModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData) : base(rules, roles, cachedData) { }
    public Models.SummaryRole Role { get; set; } = default!;
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await rules.TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!rules.CanModifyRolePermissions(role.Name!)) { return NotAllowedModify(); }
        Role = Models.SummaryRole.FromRole(role);
        var permissions = await rules.TryGetPermissionClaimAsync(role);
        if (permissions is not null && permissions.Value.Length > 0)
        {
            SelectedPermissions = Requirements.PermissionsStringToIndices(permissions.Value, cachedData.SortedPermissions);
        }
        else
        {
            SelectedPermissions = Array.Empty<int>();
        }
        return Page();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Permissions.Identity;

[RequirePermission(Places.PermissionsIdentity, Actions.Read)]
public class DetailsModel : CrudPageModel
{
    public DetailsModel(AdminRules rules, UserManager<Models.Identity> users, CachedPermissions cachedData) : base(rules, users, cachedData) { }
    public Models.SummaryIdentity Identity { get; set; } = default!;
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var identity = await rules.TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await rules.CanModifyUserPermissionsAsync(identity)) { return NotAllowedModify(); }
        Identity = Models.SummaryIdentity.FromIdentity(identity);
        var permissions = await rules.TryGetPermissionClaimAsync(identity);
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

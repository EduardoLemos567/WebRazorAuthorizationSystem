using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Permissions.Identity;

public class DetailsModel : CrudPageModel
{
    public Models.SummaryIdentity Identity { get; set; } = default!;
    public IList<int> SelectedPermissions { get; set; } = default!;
    public DetailsModel(UserManager<Models.Identity> users, CachedDefaultData cachedData) : base(users, cachedData) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var identity = await TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await CanModifyPermissionsAsync(identity)) { return NotAllowedModify(); }
        Identity = Models.SummaryIdentity.FromIdentity(identity);
        var oldClaim = await TryFindOldClaimAsync(identity);
        if (oldClaim is not null && oldClaim.Value.Length > 0)
        {
            SelectedPermissions = Requirements.PermissionsStringToIndices(oldClaim.Value, cachedData.SortedPermissions);
        }
        else
        {
            SelectedPermissions = Array.Empty<int>();
        }
        return Page();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Identity;

public class EditModel : CrudPageModel
{
    public Models.SummaryIdentity Identity { get; set; } = default!;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public EditModel(UserManager<Models.Identity> users, CachedDefaultData cachedData) : base(users, cachedData) { }
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
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (SelectedPermissionsIsInvalid())
        {
            ModelState.AddModelError("SelectedPermissions", "Incorrect selected permissions");
        }
        var identity = await TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await CanModifyPermissionsAsync(identity)) { return NotAllowedModify(); }
        if (!ModelState.IsValid)
        {
            Identity = Models.SummaryIdentity.FromIdentity(identity);
            return Page();
        }
        var oldClaim = await TryFindOldClaimAsync(identity);
        var newClaim = new Claim(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            Requirements.PermissionsIndicesToString(SelectedPermissions, cachedData.SortedPermissions)
        );
        if (oldClaim is not null)
        {
            var removeClaimResult = await users.RemoveClaimAsync(identity, oldClaim);
            if (!removeClaimResult.Succeeded)
            {
                return SavePermissionError(string.Join(", ", from e in removeClaimResult.Errors select e.Description));
            }
        }
        var addClaimResult = await users.AddClaimAsync(identity, newClaim);
        if (!addClaimResult.Succeeded)
        {
            return SavePermissionError(string.Join(", ", from e in addClaimResult.Errors select e.Description));
        }
        // TODO: Cause user to reconnect and obtain claims
        return RedirectToPage("./Index");
    }
    private ContentResult SavePermissionError(string reasons)
    {
        return Content($"Could not save permissions, try again.\nReasons: {reasons}");
    }
    private bool SelectedPermissionsIsInvalid() => (from idx in SelectedPermissions where idx < 0 || idx >= Permissions.Count select idx).Any();
}

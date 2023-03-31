using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Models;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Identity;

public class EditModel : CrudPageModel
{
    public EditModel(UserManager<Models.Identity> users, CachedDefaultData cachedData) : base(users, cachedData) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var identity = await TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await CanModifyPermissionsAsync(identity)) { return NotAllowedModify(); }
        Identity = Models.SummaryIdentity.FromIdentity(identity);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (SelectedPermissionsIsInvalid())
        {
            ModelState.AddModelError("SelectedPermissions", "Incorrect selected permissions");
        }
        if (!ModelState.IsValid) { return Page(); }
        var identity = await TryFindUserAsync(Identity.Id);
        if (identity is null) { return NotFound(); }
        if (!await CanModifyPermissionsAsync(identity)) { return NotAllowedModify(); }
        var oldClaim = await TryFindOldClaimAsync(identity);
        var newClaim = CollectPermissionsIntoClaim();
        var modifyClaimResult = oldClaim is null ?
                await users.AddClaimAsync(identity, newClaim)
                : await users.ReplaceClaimAsync(identity, oldClaim, newClaim);
        if (!modifyClaimResult.Succeeded)
        {
            return Content("Could not save permissions, try again." +
                $"\nReasons: {string.Join(", ", from e in modifyClaimResult.Errors select e.Description)}");
        }
        // TODO: Cause user to reconnect and obtain claims
        return RedirectToPage("./Index");
    }
    private async Task<Claim?> TryFindOldClaimAsync(Models.Identity user)
    {
        return (await users.GetClaimsAsync(user))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
    }
    private Claim CollectPermissionsIntoClaim()
    {
        return new(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            new((from i in SelectedPermissions select cachedData.SortedPermissions[i].data).ToArray())
        );
    }
    private bool SelectedPermissionsIsInvalid() => (from idx in SelectedPermissions where idx < 0 || idx >= SelectedPermissions.Count select idx).Any();
}

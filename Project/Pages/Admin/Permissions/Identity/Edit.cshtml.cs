using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;
using Project.Utils;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Identity;

[RequirePermission(Places.PermissionsIdentity, Actions.Update)]
public class EditModel : CrudPageModel
{
    public EditModel(AdminRules rules, UserManager<Models.Identity> users, CachedPermissions cachedData) : base(rules, users, cachedData) { }
    public Models.SummaryIdentity Identity { get; set; } = default!;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var identity = await rules.TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await rules.CanModifyUserPermissionsAsync(identity)) { return NotAllowedModify(); }
        Identity = Models.SummaryIdentity.FromIdentity(identity);
        var oldClaim = await rules.TryGetPermissionClaimAsync(identity);
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
        if (!Util.SelectionIsValid(SelectedPermissions, AllPermissions.Count))
        {
            ModelState.AddModelError("SelectedPermissions", "Incorrect selected permissions");
        }
        var identity = await rules.TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await rules.CanModifyUserPermissionsAsync(identity)) { return NotAllowedModify(); }
        if (!ModelState.IsValid)
        {
            Identity = Models.SummaryIdentity.FromIdentity(identity);
            return Page();
        }
        var permissions = await rules.TryGetPermissionClaimAsync(identity);
        var newPermissions = new Claim(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            Requirements.PermissionsIndicesToString(SelectedPermissions, cachedData.SortedPermissions)
        );
        if (permissions is not null)
        {
            var removeClaimResult = await users.RemoveClaimAsync(identity, permissions);
            if (!removeClaimResult.Succeeded)
            {
                return SavePermissionError(string.Join(", ", from e in removeClaimResult.Errors select e.Description));
            }
        }
        var addClaimResult = await users.AddClaimAsync(identity, newPermissions);
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
}

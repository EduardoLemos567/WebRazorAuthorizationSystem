using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Role;

public class EditModel : CrudPageModel
{
    public Models.SummaryRole Role { get; set; } = default!;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public EditModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!CanModifyPermissions(role)) { return NotAllowedModify(); }
        Role = Models.SummaryRole.FromRole(role);
        var oldClaim = await TryFindOldClaimAsync(role);
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
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!CanModifyPermissions(role)) { return NotAllowedModify(); }
        if (!ModelState.IsValid)
        {
            Role = Models.SummaryRole.FromRole(role);
            return Page();
        }
        var oldClaim = await TryFindOldClaimAsync(role);
        var newClaim = new Claim(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            Requirements.PermissionsIndicesToString(SelectedPermissions, cachedData.SortedPermissions)
        );
        if (oldClaim is not null)
        {
            var removeClaimResult = await roles.RemoveClaimAsync(role, oldClaim);
            if (!removeClaimResult.Succeeded)
            {
                return SavePermissionError(string.Join(", ", from e in removeClaimResult.Errors select e.Description));
            }
        }
        var addClaimResult = await roles.AddClaimAsync(role, newClaim);
        if (!addClaimResult.Succeeded)
        {
            return SavePermissionError(string.Join(", ", from e in addClaimResult.Errors select e.Description));
        }
        return RedirectToPage("./Index");
    }
    private ContentResult SavePermissionError(string reasons)
    {
        return Content($"Could not save permissions, try again.\nReasons: {reasons}");
    }
    private bool SelectedPermissionsIsInvalid() => (from idx in SelectedPermissions where idx < 0 || idx >= Permissions.Count select idx).Any();
}

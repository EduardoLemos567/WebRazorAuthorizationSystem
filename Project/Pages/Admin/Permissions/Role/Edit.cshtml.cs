using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;
using Project.Utils;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Role;

[RequirePermission(Places.PermissionsRole, Actions.Update)]
public class EditModel : CrudPageModel
{
    public EditModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData) : base(rules, roles, cachedData) { }
    public Models.SummaryRole Role { get; set; } = default!;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await rules.TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!rules.CanModifyRolePermissions(role.Name!)) { return NotAllowedModify(); }
        Role = Models.SummaryRole.FromRole(role);
        var oldClaim = await rules.TryGetPermissionClaimAsync(role);
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
        if (Util.SelectionIsInvalid(SelectedPermissions, AllPermissions.Count))
        {
            ModelState.AddModelError("SelectedPermissions", "Incorrect selected permissions");
        }
        var role = await rules.TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!rules.CanModifyRolePermissions(role.Name!)) { return NotAllowedModify(); }
        if (!ModelState.IsValid)
        {
            Role = Models.SummaryRole.FromRole(role);
            return Page();
        }
        var permissions = await rules.TryGetPermissionClaimAsync(role);
        if (permissions is not null)
        {
            var removeClaimResult = await roles.RemoveClaimAsync(role, permissions);
            if (!removeClaimResult.Succeeded)
            {
                return SavePermissionError(string.Join(", ", from e in removeClaimResult.Errors select e.Description));
            }
        }
        permissions = new Claim(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            Requirements.PermissionsIndicesToString(SelectedPermissions, cachedData.SortedPermissions)
        );
        var addClaimResult = await roles.AddClaimAsync(role, permissions);
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
}

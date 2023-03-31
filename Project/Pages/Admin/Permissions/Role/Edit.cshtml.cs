using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Role;

public class EditModel : CrudPageModel
{
    public EditModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!CanModifyPermissions(role)) { return NotAllowedModify(); }
        Role = Models.SummaryRole.FromRole(role);
        var oldClaim = await TryFindOldClaimAsync(role);
        if (oldClaim is not null && oldClaim.Value.Length > 0)
        {
            var permissionsChar = oldClaim.Value.ToArray();
            Array.Sort(permissionsChar);
            var selected = new int[permissionsChar.Length];
            //TODO here and modify Users counterpart
        }
        else
        {
            SelectedPermissions = Array.Empty<int>();
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (SelectedPermissionsIsInvalid())
        {
            ModelState.AddModelError("SelectedPermissions", "Incorrect selected permissions");
        }
        if (!ModelState.IsValid) { return Page(); }
        var role = await TryFindRoleAsync(Role.Id);
        if (role is null) { return NotFound(); }
        if (!CanModifyPermissions(role)) { return NotAllowedModify(); }
        var oldClaim = await TryFindOldClaimAsync(role);
        var newClaim = CollectPermissionsIntoClaim();
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
    public ContentResult SavePermissionError(string reasons)
    {
        return Content($"Could not save permissions, try again.\nReasons: {reasons}");
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

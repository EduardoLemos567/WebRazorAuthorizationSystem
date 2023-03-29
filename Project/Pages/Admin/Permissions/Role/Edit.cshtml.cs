using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Role;

public class EditModel : PageModel
{
    private readonly RoleManager<Models.Role> roles;
    private readonly CachedDefaultData permissions;
    public EditModel(RoleManager<Models.Role> roles, CachedDefaultData permissions)
    {
        this.roles = roles;
        this.permissions = permissions;
    }
    public Models.Role Role { get; set; } = default!;
    public IReadOnlyList<string> Permissions => permissions.SortedPermissionsStrings;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var role = await roles.FindByIdAsync(id.ToString()!);
        // Identity not found or identity is not in default role 'staff'.
        if (role is null)
        {
            return NotFound();
        }
        if (role.Name == DefaultRoles.User.ToString())
        {
            return Content("Role 'User' cannot have permissions.");
        }
        Role = role;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if ((from idx in SelectedPermissions where idx >= 0 || idx < SelectedPermissions.Count select idx).Any())
        {
            ModelState.AddModelError("SelectedPermissions", "Incorrect selected permissions");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var oldClaim = (await roles.GetClaimsAsync(Role))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
        var newClaim = new Claim(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            new((from i in SelectedPermissions select permissions.SortedPermissions[i].data).ToArray())
        );
        if (oldClaim is not null)
        {
            var removeClaimResult = await roles.RemoveClaimAsync(Role, oldClaim);
            if (!removeClaimResult.Succeeded)
            {
                return SavePermissionError(string.Join(", ", from e in removeClaimResult.Errors select e.Description));
            }
        }
        var addClaimResult = await roles.AddClaimAsync(Role, newClaim);
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
}

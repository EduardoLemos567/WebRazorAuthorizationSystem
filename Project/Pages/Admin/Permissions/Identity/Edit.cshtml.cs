using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using System.Security.Claims;

namespace Project.Pages.Admin.Permissions.Identity;

public class EditModel : PageModel
{
    private readonly UserManager<Models.Identity> users;
    private readonly CachedDefaultData cachedData;
    public EditModel(UserManager<Models.Identity> users, CachedDefaultData permissions)
    {
        this.users = users;
        this.cachedData = permissions;
    }
    public Models.Identity Identity { get; set; } = default!;
    public IReadOnlyList<string> Permissions => cachedData.SortedPermissionsStrings;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var identity = await users.FindByIdAsync(id.ToString()!);
        if (identity is null)
        {
            return NotFound();
        }
        if (!await users.IsInRoleAsync(identity, DefaultRoles.Staff.ToString()))
        {
            return Content("User is not member of Staff, cant have permissions.");
        }
        Identity = identity;
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
        //TODO: Check user id ? maybe not necessary.
        var oldClaim = (await users.GetClaimsAsync(Identity))
            .Where(c => c.Type == Requirements.PERMISSIONS_CLAIM_TYPE)
            .FirstOrDefault();
        var newClaim = new Claim(
            Requirements.PERMISSIONS_CLAIM_TYPE,
            new((from i in SelectedPermissions select cachedData.SortedPermissions[i].data).ToArray())
        );
        IdentityResult modifyClaimResult;
        if (oldClaim is null)
        {
            modifyClaimResult = await users.AddClaimAsync(Identity, newClaim);
        }
        else
        {
            modifyClaimResult = await users.ReplaceClaimAsync(Identity, oldClaim, newClaim);
        }
        if (!modifyClaimResult.Succeeded)
        {
            return Content("Could not save permissions, try again." +
                $"\nReasons: {string.Join(", ", from e in modifyClaimResult.Errors select e.Description)}");
        }
        // TODO: Cause user to reconnect and obtain claims
        return RedirectToPage("./Index");
    }
}

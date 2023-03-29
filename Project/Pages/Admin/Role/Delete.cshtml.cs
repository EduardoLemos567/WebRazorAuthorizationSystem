using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class DeleteModel : PageModel
{
    private readonly RoleManager<Models.Role> roles;
    private readonly CachedDefaultData cachedData;
    public DeleteModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData)
    {
        this.roles = roles;
        this.cachedData = cachedData;
    }
    [BindProperty]
    public Models.Role Role { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var role = await roles.FindByIdAsync(id.ToString()!);
        if (role is null)
        {
            return NotFound();
        }
        if (cachedData.SortedDefaultRoles.Contains(role.Name))
        {
            return Content("Cannot delete any of DefaultRoles.");
        }
        Role = role;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var role = await roles.FindByIdAsync(id.ToString()!);
        if (role is null)
        {
            return NotFound();
        }
        if (cachedData.SortedDefaultRoles.Contains(role.Name))
        {
            return Content("Cannot delete any of DefaultRoles.");
        }
        var deletionResult = await roles.DeleteAsync(role);
        if (!deletionResult.Succeeded)
        {
            return Content($"Could not delete role. Reasons {string.Join(", ",
                from e in deletionResult.Errors select e.Description)}");
        }
        Role = role;
        return RedirectToPage("./Index");
    }
}

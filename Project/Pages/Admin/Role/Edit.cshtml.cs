using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class EditModel : PageModel
{
    private readonly RoleManager<Models.Role> roles;
    private readonly CachedDefaultData cachedData;
    public EditModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData)
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
            return Content("Cannot edit any of DefaultRoles.");
        }
        Role = role;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (cachedData.SortedDefaultRoles.Contains(Role.Name))
        {
            return Content("Cannot edit any of DefaultRoles.");
        }
        var result = await roles.UpdateAsync(Role);
        if (!result.Succeeded)
        {
            return Content($"Couldnt update the role. Reasons: {string.Join(", ", from e in result.Errors select e.Description)}");
        }
        return RedirectToPage("./Index");
    }
}

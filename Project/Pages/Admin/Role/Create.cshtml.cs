using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.Role;

public class CreateModel : PageModel
{
    private readonly RoleManager<Models.Role> roles;
    public CreateModel(RoleManager<Models.Role> roles)
    {
        this.roles = roles;
    }
    [BindProperty]
    public Models.Role Role { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (await roles.RoleExistsAsync(Role.Name!))
        {
            return Content("Role already exists.");
        }
        var creationResult = await roles.CreateAsync(Role);
        if (!creationResult.Succeeded)
        {
            return Content($"Could not create role. Reasons {string.Join(", ",
                from e in creationResult.Errors select e.Description)}");
        }
        return RedirectToPage("./Index");
    }
}

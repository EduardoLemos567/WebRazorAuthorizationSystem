using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class CreateModel : CrudPageModel
{
    public CreateModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        if (await roles.RoleExistsAsync(Role.Name!)) { return Content("Role already exists."); }
        var role = new Models.Role();
        Role.Update(role);
        var creationResult = await roles.CreateAsync(role);
        if (!creationResult.Succeeded)
        {
            return Content($"Could not create role. Reasons {string.Join(", ",
                from e in creationResult.Errors select e.Description)}");
        }
        return RedirectToPage("./Index");
    }
}

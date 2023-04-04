using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Role;

[RequirePermission(Places.Role, Actions.Create)]
public class CreateModel : CrudPageModel
{
    public CreateModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData) : base(rules, roles, cachedData) { }
    [BindProperty]
    public Models.SummaryRole Role { get; set; } = default!;
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

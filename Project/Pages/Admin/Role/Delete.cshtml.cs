using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class DeleteModel : CrudPageModel
{
    public DeleteModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (cachedData.IsDefaultRole(role)) { return CantDeleteDefault(); }
        Role = Models.SummaryRole.FromRole(role);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (cachedData.IsDefaultRole(role)) { return CantDeleteDefault(); }
        var deletionResult = await roles.DeleteAsync(role);
        if (!deletionResult.Succeeded)
        {
            return Content($"Could not delete role. Reasons {string.Join(", ",
                from e in deletionResult.Errors select e.Description)}");
        }
        return RedirectToPage("./Index");
    }
    private IActionResult CantDeleteDefault() => Content("Cannot delete any of DefaultRoles.");
}

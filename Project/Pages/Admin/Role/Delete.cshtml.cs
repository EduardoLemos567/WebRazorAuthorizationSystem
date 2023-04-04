using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Role;

[RequirePermission(Places.Role, Actions.Delete)]
public class DeleteModel : CrudPageModel
{
    public DeleteModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData) : base(rules, roles, cachedData) { }
    public Models.SummaryRole Role { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await rules.TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!rules.CanCRUDRole(role.Name!)) { return CantDeleteDefault(); }
        Role = Models.SummaryRole.FromRole(role);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var role = await rules.TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (!rules.CanCRUDRole(role.Name!)) { return CantDeleteDefault(); }
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

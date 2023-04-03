using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class EditModel : CrudPageModel
{
    public EditModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    [BindProperty]
    public Models.SummaryRole Role { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        if (cachedData.IsDefaultRole(role)) { return CantEditDefault(); }
        Role = Models.SummaryRole.FromRole(role);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        var role = await TryFindRoleAsync(Role.Id);
        if (role is null) { return NotFound(); }
        if (cachedData.IsDefaultRole(role)) { return CantEditDefault(); }
        Role.Update(role);
        var result = await roles.UpdateAsync(role);
        if (!result.Succeeded)
        {
            return Content($"Couldnt update the role. Reasons: {string.Join(", ",
                from e in result.Errors select e.Description)}");
        }
        return RedirectToPage("./Index");
    }
    private ContentResult CantEditDefault() => Content("Cannot edit any of DefaultRoles.");
}

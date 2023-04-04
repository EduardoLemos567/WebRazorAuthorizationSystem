using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Role;

[RequirePermission(Places.Role, Actions.Read)]
public class DetailsModel : CrudPageModel
{
    public DetailsModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData) : base(rules, roles, cachedData) { }
    public Models.SummaryRole Role { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await rules.TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        Role = Models.SummaryRole.FromRole(role);
        return Page();
    }
}

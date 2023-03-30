using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class DetailsModel : CrudPageModel
{
    public DetailsModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var role = await TryFindRoleAsync(id);
        if (role is null) { return NotFound(); }
        Role = Models.SummaryRole.FromRole(role);
        return Page();
    }
}

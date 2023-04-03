using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class CrudPageModel : PageModel
{
    protected readonly RoleManager<Models.Role> roles;
    protected readonly CachedDefaultData cachedData;
    public CrudPageModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData)
    {
        this.roles = roles;
        this.cachedData = cachedData;
    }
    protected async Task<Models.Role?> TryFindRoleAsync(int? id)
    {
        if (id is null) { return null; }
        return await roles.FindByIdAsync(id.ToString()!);
    }
}

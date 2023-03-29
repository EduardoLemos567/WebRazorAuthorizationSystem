using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class IndexModel : PageModel
{
    private readonly RoleManager<Models.Role> roles;
    private readonly CachedDefaultData cachedData;
    public IndexModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData)
    {
        this.roles = roles;
        this.cachedData = cachedData;
    }
    public IList<Models.Role> Roles { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Roles = await roles.Roles.ExceptBy(cachedData.SortedDefaultRoles, r => r.Name).ToListAsync();
    }
}

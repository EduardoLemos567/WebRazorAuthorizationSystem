using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Permissions.Role;

[RequirePermission(Places.PermissionsRole, Actions.List)]
public class IndexModel : CrudPageModel
{
    public IndexModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData) : base(rules, roles, cachedData) { }
    public IList<Models.Role> Roles { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Roles = await rules.GetAllNonDefaultRoles().ToListAsync();
    }
}

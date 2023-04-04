using Microsoft.AspNetCore.Identity;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Permissions.Identity;

[RequirePermission(Places.PermissionsIdentity, Actions.List)]
public class IndexModel : CrudPageModel
{
    public IndexModel(AdminRules rules, UserManager<Models.Identity> users, CachedPermissions cachedData) : base(rules, users, cachedData) { }
    public IList<Models.Identity> Identities { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Identities = await users.GetUsersInRoleAsync(DefaultRoles.Staff);
    }
}

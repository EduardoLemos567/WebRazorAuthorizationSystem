using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Role;

public class CrudPageModel : PageModel
{
    protected readonly AdminRules rules;
    protected readonly RoleManager<Models.Role> roles;
    protected readonly CachedPermissions cachedData;
    public CrudPageModel(AdminRules rules, RoleManager<Models.Role> roles, CachedPermissions cachedData)
    {
        this.rules = rules;
        this.roles = roles;
        this.cachedData = cachedData;
    }
}

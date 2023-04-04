using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Permissions.Role;

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
    public IReadOnlyList<string> AllPermissions => cachedData.SortedPermissionsStrings;
    protected IActionResult NotAllowedModify() => Content("Role 'User' cannot have permissions.");
}

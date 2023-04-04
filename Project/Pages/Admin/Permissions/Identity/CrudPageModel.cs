using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Permissions.Identity;

public class CrudPageModel : PageModel
{
    protected readonly AdminRules rules;
    protected readonly UserManager<Models.Identity> users;
    protected readonly CachedPermissions cachedData;
    public CrudPageModel(AdminRules rules, UserManager<Models.Identity> users, CachedPermissions cachedData)
    {
        this.rules = rules;
        this.users = users;
        this.cachedData = cachedData;
    }
    public IReadOnlyList<string> AllPermissions => cachedData.SortedPermissionsStrings;
    protected IActionResult NotAllowedModify() => Content("User is not member of 'Staff', cant have permissions.");
}

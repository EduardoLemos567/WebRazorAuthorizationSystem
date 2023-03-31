using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Models;

namespace Project.Pages.Admin.Permissions.Identity;

public class CrudPageModel : PageModel
{
    protected readonly UserManager<Models.Identity> users;
    protected readonly CachedDefaultData cachedData;
    public CrudPageModel(UserManager<Models.Identity> users, CachedDefaultData cachedData)
    {
        this.users = users;
        this.cachedData = cachedData;
    }
    public SummaryIdentity Identity { get; set; } = default!;
    public IReadOnlyList<string> Permissions => cachedData.SortedPermissionsStrings;
    [BindProperty]
    public IList<int> SelectedPermissions { get; set; } = default!;
    protected async Task<Models.Identity?> TryFindUserAsync(int? id)
    {
        if (id is null) { return null; }
        return await users.FindByIdAsync(id.ToString()!);
    }
    protected Task<bool> CanModifyPermissionsAsync(Models.Identity user)
    {
        return users.IsInRoleAsync(user, DefaultRoles.Staff.ToString());
    }
    protected IActionResult NotAllowedModify() => Content("User is not member of 'Staff', cant have permissions.");
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.Enrole;

public class CrudPageModel : PageModel
{
    protected readonly UserManager<Models.Identity> users;
    protected readonly RoleManager<Models.Role> roles;
    public CrudPageModel(UserManager<Models.Identity> users, RoleManager<Models.Role> roles)
    {
        this.users = users;
        this.roles = roles;        
    }
    protected async Task<Models.Identity?> TryFindUserAsync(int? id)
    {
        if (id is null) { return null; }
        return await users.FindByIdAsync(id!.ToString()!);
    }
}

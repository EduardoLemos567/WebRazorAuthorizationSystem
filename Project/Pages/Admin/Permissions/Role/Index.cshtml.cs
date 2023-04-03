using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Permissions.Role;

public class IndexModel : CrudPageModel
{
    public IndexModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public IList<Models.Role> Roles { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Roles = await roles.Roles.Where(r =>
            r.Name != DefaultRoles.User.ToString()
            && r.Name != DefaultRoles.Admin.ToString()
        ).ToListAsync();
    }
}

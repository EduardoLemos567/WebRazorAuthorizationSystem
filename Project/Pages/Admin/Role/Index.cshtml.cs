using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;

namespace Project.Pages.Admin.Role;

public class IndexModel : CrudPageModel
{
    public IndexModel(RoleManager<Models.Role> roles, CachedDefaultData cachedData) : base(roles, cachedData) { }
    public IList<Models.Role> Roles { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Roles = await (from r in roles.Roles where !cachedData.IsDefaultRole(r) select r).ToListAsync();
    }
}

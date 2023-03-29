using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Permissions.Role;

public class IndexModel : PageModel
{
    private readonly DataDbContext db;
    public IndexModel(DataDbContext context)
    {
        db = context;
    }
    public IList<Models.Role> Roles { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Roles = await db.Roles.Where(r => r.Name != DefaultRoles.User.ToString()).ToListAsync();
    }
}

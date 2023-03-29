using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Permissions.Role;

public class DetailsModel : PageModel
{
    private readonly DataDbContext db;
    public DetailsModel(DataDbContext context)
    {
        db = context;
    }
    public Models.Role Role { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var role = await db.Roles.FindAsync(id);
        if (role is null)
        {
            return NotFound();
        }
        if (role.Name == DefaultRoles.User.ToString())
        {
            return Content("Role 'User' cannot have permissions.");
        }
        Role = role;
        return Page();
    }
}

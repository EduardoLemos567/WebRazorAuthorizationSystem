using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.Role;

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
        Role = role;
        return Page();
    }
}

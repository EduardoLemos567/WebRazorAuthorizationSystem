using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Role;

public class DetailsModel : PageModel
{
    private readonly DataDbContext _context;

    public DetailsModel(DataDbContext context)
    {
        _context = context;
    }

    public Models.Role Role { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Roles == null)
        {
            return NotFound();
        }

        var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
        if (role == null)
        {
            return NotFound();
        }
        else
        {
            Role = role;
        }
        return Page();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Role;

public class DeleteModel : PageModel
{
    private readonly DataDbContext _context;

    public DeleteModel(DataDbContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Roles == null)
        {
            return NotFound();
        }
        var role = await _context.Roles.FindAsync(id);

        if (role != null)
        {
            Role = role;
            _context.Roles.Remove(Role);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

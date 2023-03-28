using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Role;

public class IndexModel : PageModel
{
    private readonly DataDbContext _context;

    public IndexModel(DataDbContext context)
    {
        _context = context;
    }

    public IList<Models.Role> Roles { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Roles != null)
        {
            Roles = await _context.Roles.ToListAsync();
        }
    }
}

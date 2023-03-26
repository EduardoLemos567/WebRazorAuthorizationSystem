using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.PermissionsPackage;

public class IndexModel : PageModel
{
    private readonly DataDbContext _context;

    public IndexModel(DataDbContext context)
    {
        _context = context;
    }
    public IList<Models.PermissionsPackage> PermissionsPackages { get; set; } = default!;
    public async Task OnGetAsync()
    {
        if (_context.PermissionsPackages != null)
        {
            PermissionsPackages = await _context.PermissionsPackages.ToListAsync();
        }
    }
}

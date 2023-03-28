using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

public class IndexModel : PageModel
{
    private readonly DataDbContext _context;

    public IndexModel(DataDbContext context)
    {
        _context = context;
    }

    public IList<Models.MovieCategory> MovieCategories { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.MovieCategories != null)
        {
            MovieCategories = await _context.MovieCategories.ToListAsync();
        }
    }
}

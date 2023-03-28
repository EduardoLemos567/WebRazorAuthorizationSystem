using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

public class DetailsModel : PageModel
{
    private readonly DataDbContext _context;

    public DetailsModel(DataDbContext context)
    {
        _context = context;
    }

    public Models.MovieCategory MovieCategory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.MovieCategories == null)
        {
            return NotFound();
        }

        var moviecategory = await _context.MovieCategories.FirstOrDefaultAsync(m => m.Id == id);
        if (moviecategory == null)
        {
            return NotFound();
        }
        else
        {
            MovieCategory = moviecategory;
        }
        return Page();
    }
}

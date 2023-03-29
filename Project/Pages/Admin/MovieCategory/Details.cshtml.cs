using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Read)]
public class DetailsModel : PageModel
{
    private readonly DataDbContext db;
    public DetailsModel(DataDbContext context)
    {
        db = context;
    }
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var moviecategory = await db.MovieCategories.FindAsync(id);
        if (moviecategory is null)
        {
            return NotFound();
        }
        MovieCategory = moviecategory;
        return Page();
    }
}

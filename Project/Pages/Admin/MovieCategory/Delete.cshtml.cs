using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Delete)]
public class DeleteModel : PageModel
{
    private readonly DataDbContext db;
    public DeleteModel(DataDbContext context)
    {
        db = context;
    }
    [BindProperty]
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
    public async Task<IActionResult> OnPostAsync(int? id)
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
        db.MovieCategories.Remove(MovieCategory);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

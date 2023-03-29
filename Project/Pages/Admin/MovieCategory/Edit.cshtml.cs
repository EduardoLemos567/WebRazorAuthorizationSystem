using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Update)]
public class EditModel : PageModel
{
    private readonly DataDbContext db;
    public EditModel(DataDbContext context)
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
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        db.Attach(MovieCategory).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieCategoryExists(MovieCategory.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToPage("./Index");
    }
    private bool MovieCategoryExists(int id)
    {
        return db.MovieCategories.Any(e => e.Id == id);
    }
}

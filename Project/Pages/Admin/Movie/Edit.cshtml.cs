using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class EditModel : PageModel
{
    private readonly DataDbContext db;
    public EditModel(DataDbContext context)
    {
        db = context;
    }
    [BindProperty]
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var movie = await db.Movies.FindAsync(id);
        if (movie is null)
        {
            return NotFound();
        }
        Movie = movie;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        db.Attach(Movie).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(Movie.Id))
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
    private bool MovieExists(int id) => db.Movies.Any(e => e.Id == id);
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class CreateModel : PageModel
{
    private readonly DataDbContext db;
    public CreateModel(DataDbContext context)
    {
        db = context;
    }
    [BindProperty]
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        db.Movies.Add(Movie);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

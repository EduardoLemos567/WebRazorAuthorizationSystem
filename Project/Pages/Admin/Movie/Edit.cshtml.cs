using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Permissions;

namespace Project.Pages.Admin.Movie;

[HasPermission(Places.Movie, Actions.Update)]
public class EditModel : PageModel
{
    private readonly Data.DataContext db;

    public EditModel(Data.DataContext context) => this.db = context;
    [BindProperty]
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || this.db.Movies == null)
        {
            return this.NotFound();
        }
        var movie = await this.db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return this.NotFound();
        }
        this.Movie = movie;
        return this.Page();
    }
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }
        this.db.Attach(this.Movie).State = EntityState.Modified;
        try
        {
            _ = await this.db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.MovieExists(this.Movie.Id))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }
        return this.RedirectToPage("./Index");
    }
    private bool MovieExists(int id) => this.db.Movies.Any(e => e.Id == id);
}

using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class DeleteModel : CrudPageModel
{
    public DeleteModel(DataDbContext db) : base(db) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var movie = await this.TryFindMovieAsync(id);
        if (movie is null) { return NotFound(); }
        Movie = movie;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var movie = await this.TryFindMovieAsync(id);
        if (movie is null) { return NotFound(); }
        db.Movies.Remove(movie);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

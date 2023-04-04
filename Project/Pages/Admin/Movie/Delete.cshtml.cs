using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Movie;

[RequirePermission(Places.Movie, Actions.Delete)]
public class DeleteModel : CrudPageModel
{
    public DeleteModel(DataDbContext db) : base(db) { }
    public Models.Movie Movie { get; set; } = default!;
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

using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class CrudPageModel : PageModel
{
    protected readonly DataDbContext db;
    public CrudPageModel(DataDbContext db) => this.db = db;
    protected async Task<Models.Movie?> TryFindMovieAsync(int? id)
    {
        if (id is null) { return null; }
        return await db.Movies.FindAsync(id);
    }
}

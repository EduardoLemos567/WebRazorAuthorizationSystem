using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class DetailsModel : CrudPageModel
{
    public DetailsModel(DataDbContext db) : base(db) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var movie = await this.TryFindMovieAsync(id);
        if (movie is null) { return NotFound(); }
        Movie = movie;
        return Page();
    }
}

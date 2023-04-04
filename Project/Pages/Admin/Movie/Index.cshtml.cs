using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Movie;

[RequirePermission(Places.Movie, Actions.List)]
public class IndexModel : CrudPageModel
{
    public IndexModel(DataDbContext db) : base(db) { }
    public IList<Models.Movie> Movies { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Movies = await db.Movies.ToListAsync();
    }
}

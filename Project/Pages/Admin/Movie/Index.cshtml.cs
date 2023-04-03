using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class IndexModel : CrudPageModel
{
    public IndexModel(DataDbContext db) : base(db) { }
    public IList<Models.Movie> Movies { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Movies = await db.Movies.ToListAsync();
    }
}

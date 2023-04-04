using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.List)]
public class IndexModel : CrudPageModel
{
    public IndexModel(DataDbContext db) : base(db) { }
    public IList<Models.MovieCategory> MovieCategories { get; set; } = default!;
    public async Task OnGetAsync()
    {
        MovieCategories = await db.MovieCategories.ToListAsync();
    }
}

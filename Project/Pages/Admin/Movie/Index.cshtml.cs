using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Permissions;

namespace Project.Pages.Admin.Movie;

[HasPermission(Places.Movie, Actions.List)]
public partial class IndexModel : PageModel
{
    private readonly Data.DataDbContext db;
    public IndexModel(Data.DataDbContext context) => this.db = context;
    public IList<Models.Movie> Movies { get; set; } = default!;
    public async Task OnGetAsync() => this.Movies = await this.db.Movies.ToListAsync();
}

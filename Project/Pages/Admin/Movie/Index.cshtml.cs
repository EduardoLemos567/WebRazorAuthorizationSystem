using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Admin.Movie;

public class IndexModel : PageModel
{
    private readonly Data.SauceContext db;
    public IndexModel(Data.SauceContext context) => this.db = context;
    public IList<Models.Movie> Movies { get; set; } = default!;
    public async Task OnGetAsync() => this.Movies = await this.db.Movies.ToListAsync();
}

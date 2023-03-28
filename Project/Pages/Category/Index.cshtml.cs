using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Category;

public class IndexModel : PageModel
{
    private readonly Data.DataDbContext db;
    public IndexModel(Data.DataDbContext context) => this.db = context;
    public IList<Models.MovieCategory> MovieCategories { get; set; } = default!;
    public async Task OnGetAsync() => this.MovieCategories = await this.db.MovieCategories.ToListAsync();
}

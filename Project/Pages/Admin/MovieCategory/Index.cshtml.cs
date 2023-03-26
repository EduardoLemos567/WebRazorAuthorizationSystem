using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Admin.MovieCategory;

public class IndexModel : PageModel
{
    private readonly Data.DataDbContext db;
    public IndexModel(Data.DataDbContext context) => this.db = context;
    public IList<Models.MovieCategory> MovieCategory { get; set; } = default!;
    public async Task OnGetAsync()
    {
        if (this.db.MovieCategories != null)
        {
            this.MovieCategory = await this.db.MovieCategories.ToListAsync();
        }
    }
}

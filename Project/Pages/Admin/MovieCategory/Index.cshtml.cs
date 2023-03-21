using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Admin.MovieCategory;

public class IndexModel : PageModel
{
    private readonly Data.SauceContext db;
    public IndexModel(Data.SauceContext context) => this.db = context;
    public IList<Models.MovieCategory> MovieCategory { get; set; } = default!;
    public async Task OnGetAsync()
    {
        if (this.db.MovieCategories != null)
        {
            this.MovieCategory = await this.db.MovieCategories.ToListAsync();
        }
    }
}

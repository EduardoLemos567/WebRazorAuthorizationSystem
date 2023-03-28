using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Movie
{
    public class WatchModel : PageModel
    {
        private readonly Data.DataDbContext db;
        public WatchModel(Data.DataDbContext context) => this.db = context;
        public Models.Movie Movie { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await this.db.Movies.FindAsync(id);
            if(result is null)
            {
                return Content("Movie not found");
            }
            Movie = result;
            return Page();
        }
    }
}

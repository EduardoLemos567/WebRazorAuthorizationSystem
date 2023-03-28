using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Category
{
    public class ViewModel : PageModel
    {
        private readonly Data.DataDbContext db;
        public ViewModel(Data.DataDbContext context) => this.db = context;
        public Models.MovieCategory MovieCategory { get; set; } = default!;
        public IList<Models.Movie> Movies { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await this.db.MovieCategories.FindAsync(id);
            if (result is null)
            {
                return Content("Category not found");
            }
            MovieCategory = result;
            this.Movies = await (from movie in this.db.Movies where movie.Category == result select movie).ToListAsync();
            return Page();
        }
    }
}

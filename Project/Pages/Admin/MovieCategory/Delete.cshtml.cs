using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Admin.MovieCategory;

public class DeleteModel : PageModel
{
    private readonly Data.SauceContext db;
    public DeleteModel(Data.SauceContext context) => this.db = context;
    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || this.db.MovieCategories == null)
        {
            return this.NotFound();
        }
        var moviecategory = await this.db.MovieCategories.FirstOrDefaultAsync(m => m.Id == id);
        if (moviecategory == null)
        {
            return this.NotFound();
        }
        else
        {
            this.MovieCategory = moviecategory;
        }
        return this.Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || this.db.MovieCategories == null)
        {
            return this.NotFound();
        }
        var moviecategory = await this.db.MovieCategories.FindAsync(id);
        if (moviecategory != null)
        {
            this.MovieCategory = moviecategory;
            this.db.MovieCategories.Remove(this.MovieCategory);
            _ = await this.db.SaveChangesAsync();
        }
        return this.RedirectToPage("./Index");
    }
}

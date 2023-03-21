using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Admin.MovieCategory;

public class EditModel : PageModel
{
    private readonly Data.SauceContext db;
    public EditModel(Data.SauceContext context) => this.db = context;
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
        this.MovieCategory = moviecategory;
        return this.Page();
    }
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }
        this.db.Attach(this.MovieCategory).State = EntityState.Modified;
        try
        {
            _ = await this.db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieCategoryExists(this.MovieCategory.Id))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }
        return this.RedirectToPage("./Index");
    }
    private bool MovieCategoryExists(int id) => (this.db.MovieCategories?.Any(e => e.Id == id)).GetValueOrDefault();
}

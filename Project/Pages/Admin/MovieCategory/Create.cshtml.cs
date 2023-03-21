using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.MovieCategory;

public class CreateModel : PageModel
{
    private readonly Data.SauceContext db;
    public CreateModel(Data.SauceContext context) => this.db = context;
    public IActionResult OnGet() => this.Page();
    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid || this.db.MovieCategories == null || this.MovieCategory == null)
        {
            return this.Page();
        }
        this.db.MovieCategories.Add(this.MovieCategory);
        await this.db.SaveChangesAsync();
        return this.RedirectToPage("./Index");
    }
}

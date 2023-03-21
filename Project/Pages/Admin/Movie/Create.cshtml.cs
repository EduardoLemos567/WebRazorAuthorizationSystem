using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Permissions;

namespace Project.Pages.Admin.Movie;

[HasPermission(Places.Movie, Actions.Create)]
public class CreateModel : PageModel
{
    private readonly Data.SauceContext db;
    public CreateModel(Data.SauceContext context) => this.db = context;
    public IActionResult OnGet() => this.Page();
    [BindProperty]
    public Models.Movie Movie { get; set; } = default!;
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid || this.db.Movies == null || this.Movie == null)
        {
            return this.Page();
        }
        _ = this.db.Movies.Add(this.Movie);
        _ = await this.db.SaveChangesAsync();
        return this.RedirectToPage("./Index");
    }
}

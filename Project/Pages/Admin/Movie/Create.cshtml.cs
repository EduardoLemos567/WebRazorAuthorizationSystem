using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;

namespace Project.Pages.Admin.Movie;

[RequirePermission(Places.Movie, Actions.Create)]
public class CreateModel : PageModel
{
    private readonly Data.DataDbContext db;
    public CreateModel(Data.DataDbContext context) => this.db = context;
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
        this.db.Movies.Add(this.Movie);
        await this.db.SaveChangesAsync();
        return this.RedirectToPage("./Index");
    }
}

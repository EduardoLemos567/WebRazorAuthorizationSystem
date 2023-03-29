using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Create)]
public class CreateModel : PageModel
{
    private readonly DataDbContext db;
    public CreateModel(DataDbContext context)
    {
        db = context;
    }
    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        db.MovieCategories.Add(MovieCategory);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.Movie;

[RequirePermission(Places.Movie, Actions.Create)]
public class CreateModel : CrudPageModel
{
    public CreateModel(DataDbContext db) : base(db) { }
    [BindProperty]
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        db.Movies.Add(Movie);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

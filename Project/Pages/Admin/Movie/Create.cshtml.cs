using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class CreateModel : CrudPageModel
{
    public CreateModel(DataDbContext db) : base(db) { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        db.Movies.Add(Movie);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

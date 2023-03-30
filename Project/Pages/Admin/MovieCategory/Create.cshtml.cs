using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Create)]
public class CreateModel : CrudPageModel
{
    public CreateModel(DataDbContext db) : base(db) { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        db.MovieCategories.Add(MovieCategory);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

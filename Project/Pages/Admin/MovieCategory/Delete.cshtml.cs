using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Delete)]
public class DeleteModel : CrudPageModel
{
    public DeleteModel(DataDbContext db) : base(db) { }
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var moviecategory = await this.TryFindMovieCategoryAsync(id);
        if (moviecategory is null) { return NotFound(); }
        MovieCategory = moviecategory;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var moviecategory = await this.TryFindMovieCategoryAsync(id);
        if (moviecategory is null) { return NotFound(); }
        db.MovieCategories.Remove(moviecategory);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Update)]
public class EditModel : CrudPageModel
{
    public EditModel(DataDbContext db) : base(db) { }
    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var moviecategory = await TryFindMovieCategoryAsync(id);
        if (moviecategory is null) { return NotFound(); }
        MovieCategory = moviecategory;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        if (!MovieCategoryExists(MovieCategory.Id)) { return NotFound(); }
        db.MovieCategories.Update(MovieCategory);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
    private bool MovieCategoryExists(int id)
    {
        return db.MovieCategories.Any(e => e.Id == id);
    }
}

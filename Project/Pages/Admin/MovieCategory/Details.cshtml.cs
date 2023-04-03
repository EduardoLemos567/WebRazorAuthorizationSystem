using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.Read)]
public class DetailsModel : CrudPageModel
{
    public DetailsModel(DataDbContext db) : base(db) { }
    public Models.MovieCategory MovieCategory { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var moviecategory = await this.TryFindMovieCategoryAsync(id);
        if (moviecategory is null) { return NotFound(); }
        MovieCategory = moviecategory;
        return Page();
    }
}

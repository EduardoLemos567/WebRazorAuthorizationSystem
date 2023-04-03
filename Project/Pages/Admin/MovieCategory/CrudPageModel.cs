using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

public class CrudPageModel : PageModel
{
    protected readonly DataDbContext db;
    public CrudPageModel(DataDbContext db) => this.db = db;
    protected async Task<Models.MovieCategory?> TryFindMovieCategoryAsync(int? id)
    {
        if (id is null) { return null; }
        return await db.MovieCategories.FindAsync(id);
    }
}

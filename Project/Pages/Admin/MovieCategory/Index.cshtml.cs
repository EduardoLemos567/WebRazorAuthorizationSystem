﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

[RequirePermission(Places.MovieCategory, Actions.List)]
public class IndexModel : PageModel
{
    private readonly DataDbContext db;
    public IndexModel(DataDbContext context)
    {
        db = context;
    }
    public IList<Models.MovieCategory> MovieCategories { get; set; } = default!;
    public async Task OnGetAsync()
    {
        MovieCategories = await db.MovieCategories.ToListAsync();
    }
}

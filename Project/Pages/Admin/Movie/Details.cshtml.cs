﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class DetailsModel : PageModel
{
    private readonly DataDbContext db;
    public DetailsModel(DataDbContext context)
    {
        db = context;
    }
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var movie = await db.Movies.FindAsync(id);
        if (movie is null)
        {
            return NotFound();
        }
        Movie = movie;
        return Page();
    }
}

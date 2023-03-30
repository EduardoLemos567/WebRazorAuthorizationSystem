﻿using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Pages.Admin.Movie;

public class EditModel : CrudPageModel
{
    public EditModel(DataDbContext db) : base(db) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var movie = await this.TryFindMovieAsync(id);
        if (movie is null) { return NotFound(); }
        Movie = movie;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { return Page(); }
        if (!MovieExists(Movie.Id)) { return NotFound(); }
        db.Movies.Update(Movie);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
    private bool MovieExists(int id) => db.Movies.Any(e => e.Id == id);
}

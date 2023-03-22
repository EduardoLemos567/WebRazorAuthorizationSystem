﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Permissions;

namespace Project.Pages.Admin.Movie;

[HasPermission(Places.Movie, Actions.Delete)]
public class DeleteModel : PageModel
{
    private readonly Data.DataContext db;
    public DeleteModel(Data.DataContext context) => this.db = context;
    [BindProperty]
    public Models.Movie Movie { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || this.db.Movies == null)
        {
            return this.NotFound();
        }
        var movie = await this.db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return this.NotFound();
        }
        else
        {
            this.Movie = movie;
        }
        return this.Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || this.db.Movies == null)
        {
            return this.NotFound();
        }
        var movie = await this.db.Movies.FindAsync(id);
        if (movie != null)
        {
            this.Movie = movie;
            _ = this.db.Movies.Remove(this.Movie);
            _ = await this.db.SaveChangesAsync();
        }
        return this.RedirectToPage("./Index");
    }
}

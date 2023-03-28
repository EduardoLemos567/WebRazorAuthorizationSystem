using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

public class EditModel : PageModel
{
    private readonly DataDbContext _context;

    public EditModel(DataDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.MovieCategories == null)
        {
            return NotFound();
        }

        var moviecategory = await _context.MovieCategories.FirstOrDefaultAsync(m => m.Id == id);
        if (moviecategory == null)
        {
            return NotFound();
        }
        MovieCategory = moviecategory;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(MovieCategory).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieCategoryExists(MovieCategory.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool MovieCategoryExists(int id)
    {
        return (_context.MovieCategories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

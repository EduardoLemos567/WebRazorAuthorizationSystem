using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.MovieCategory;

public class CreateModel : PageModel
{
    private readonly DataDbContext _context;

    public CreateModel(DataDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Models.MovieCategory MovieCategory { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.MovieCategories == null || MovieCategory == null)
        {
            return Page();
        }

        _context.MovieCategories.Add(MovieCategory);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

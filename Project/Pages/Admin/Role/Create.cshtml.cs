using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.Role;

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
    public Models.Role Role { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Roles == null || Role == null)
        {
            return Page();
        }

        _context.Roles.Add(Role);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

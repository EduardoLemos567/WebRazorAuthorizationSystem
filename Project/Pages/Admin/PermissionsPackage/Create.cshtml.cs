using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.PermissionsPackage;

public class CreateModel : PageModel
{
    private readonly DataContext db;
    public CreateModel(DataContext context)
    {
        db = context;
    }
    public IActionResult OnGet()
    {
        return Page();
    }
    [BindProperty]
    public Models.PermissionsPackage PermissionsPackage { get; set; } = default!;
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || db.PermissionsPackages == null || PermissionsPackage == null)
        {
            return Page();
        }

        db.PermissionsPackages.Add(PermissionsPackage);
        await db.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

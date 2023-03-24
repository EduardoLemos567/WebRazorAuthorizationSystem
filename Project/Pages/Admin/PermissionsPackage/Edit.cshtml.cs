using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.PermissionsPackage;

public class EditModel : PageModel
{
    private readonly DataContext db;
    public EditModel(DataContext context)
    {
        db = context;
    }
    [BindProperty]
    public Models.PermissionsPackage PermissionsPackage { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || db.PermissionsPackages == null)
        {
            return NotFound();
        }

        var permissionspackage = await db.PermissionsPackages.FirstOrDefaultAsync(m => m.Id == id);
        if (permissionspackage == null)
        {
            return NotFound();
        }
        PermissionsPackage = permissionspackage;
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

        db.Attach(PermissionsPackage).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PermissionsPackageExists(PermissionsPackage.Id))
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

    private bool PermissionsPackageExists(int id)
    {
        return (db.PermissionsPackages?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

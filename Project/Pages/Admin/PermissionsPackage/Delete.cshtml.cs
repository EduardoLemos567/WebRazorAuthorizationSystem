using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.PermissionsPackage
{
    public class DeleteModel : PageModel
    {
        private readonly DataContext db;
        public DeleteModel(DataContext context)
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
            else
            {
                PermissionsPackage = permissionspackage;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || db.PermissionsPackages == null)
            {
                return NotFound();
            }
            var permissionspackage = await db.PermissionsPackages.FindAsync(id);

            if (permissionspackage != null)
            {
                PermissionsPackage = permissionspackage;
                db.PermissionsPackages.Remove(PermissionsPackage);
                await db.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}

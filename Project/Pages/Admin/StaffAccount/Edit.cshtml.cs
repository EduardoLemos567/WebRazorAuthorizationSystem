using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.StaffAccount;

public class EditModel : PageModel
{
    private readonly DataDbContext db;
    public EditModel(DataDbContext context)
    {
        db = context;
    }
    [BindProperty]
    public Models.StaffAccount StaffAccount { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || db.StaffAccounts == null)
        {
            return NotFound();
        }

        var account = await db.StaffAccounts.FirstOrDefaultAsync(m => m.Id == id);
        if (account == null)
        {
            return NotFound();
        }
        StaffAccount = account;
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

        db.Attach(StaffAccount).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountExists(StaffAccount.Id))
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
    private bool AccountExists(int id)
    {
        return (db.StaffAccounts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

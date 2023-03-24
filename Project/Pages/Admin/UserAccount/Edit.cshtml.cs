using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.UserAccount;

public class EditModel : PageModel
{
    private readonly DataContext db;
    public EditModel(DataContext context)
    {
        db = context;
    }
    [BindProperty]
    public Models.UserAccount UserAccount { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || db.UserAccounts == null)
        {
            return NotFound();
        }
        var account = await db.UserAccounts.FirstOrDefaultAsync(m => m.Id == id);
        if (account == null)
        {
            return NotFound();
        }
        UserAccount = account;
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

        db.Attach(UserAccount).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountExists(UserAccount.Id))
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
        return (db.UserAccounts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}

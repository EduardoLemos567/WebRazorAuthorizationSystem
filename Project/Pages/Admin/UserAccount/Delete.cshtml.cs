using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.UserAccount;

public class DeleteModel : PageModel
{
    private readonly DataContext db;
    public DeleteModel(DataContext context)
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
        else
        {
            UserAccount = account;
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || db.UserAccounts == null)
        {
            return NotFound();
        }
        var account = await db.UserAccounts.FindAsync(id);
        if (account != null)
        {
            UserAccount = account;
            db.UserAccounts.Remove(UserAccount);
            await db.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.UserAccount;

public class CreateModel : PageModel
{
    private readonly DataDbContext db;
    public CreateModel(DataDbContext context)
    {
        db = context;
    }
    public IActionResult OnGet()
    {
        return Page();
    }
    [BindProperty]
    public Models.UserAccount UserAccount { get; set; } = default!;
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || db.UserAccounts == null || UserAccount == null)
        {
            return Page();
        }
        db.UserAccounts.Add(UserAccount);
        await db.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

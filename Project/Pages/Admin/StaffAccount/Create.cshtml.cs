using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;

namespace Project.Pages.Admin.StaffAccount;

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
    public Models.StaffAccount StaffAccount { get; set; } = default!;
    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || db.StaffAccounts == null || StaffAccount == null)
        {
            return Page();
        }

        db.StaffAccounts.Add(StaffAccount);
        await db.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.StaffAccount;

public class IndexModel : PageModel
{
    private readonly DataDbContext db;
    public IndexModel(DataDbContext context)
    {
        db = context;
    }
    public IList<Models.StaffAccount> StaffAccounts { get; set; } = default!;
    public async Task OnGetAsync()
    {
        if (db.StaffAccounts != null)
        {
            StaffAccounts = await db.StaffAccounts.ToListAsync();
        }
    }
}

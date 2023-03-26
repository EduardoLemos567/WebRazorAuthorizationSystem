using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.UserAccount;

public class IndexModel : PageModel
{
    private readonly DataDbContext db;
    public IndexModel(DataDbContext context)
    {
        db = context;
    }
    public IList<Models.UserAccount> UserAccounts { get; set; } = default!;
    public async Task OnGetAsync()
    {
        if (db.UserAccounts != null)
        {
            UserAccounts = await db.UserAccounts.ToListAsync();
        }
    }
}

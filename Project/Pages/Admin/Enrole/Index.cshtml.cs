using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Pages.Admin.Enrole;

public class IndexModel : PageModel
{
    public record struct Entry(int UserId, string UserName, string RoleNames);
    private readonly DataDbContext db;
    public IndexModel(DataDbContext db) => this.db = db;
    public IList<Entry> Selection { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync()
    {
        var query = from user in this.db.Users
                    from user_role in this.db.UserRoles
                    from role in this.db.Roles
                    where user.Id == user_role.UserId && user_role.RoleId == role.Id
                    select new
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        RoleName = role.Name
                    };
        var query2 = from e in query
                     group e by e.UserId into grouped
                     select new Entry
                     {
                         UserId = grouped.First().UserId,
                         UserName = grouped.First().UserName,
                         RoleNames = string.Join(", ", grouped.Select(m => m.RoleName))
                     };
        //return Content(query2.ToQueryString());
        Selection = await query2.ToListAsync();
        return Page();
        // show user + up to 5 roles.
        //TODO: improve this query
    }
}

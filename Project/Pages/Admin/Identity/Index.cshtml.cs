using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Admin.Identity;

public class IndexModel : CrudPageModel
{
    public IndexModel(UserManager<Models.Identity> users) : base(users) { }
    public IList<Models.Identity> Identities { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Identities = await users.Users.ToListAsync();
    }
}

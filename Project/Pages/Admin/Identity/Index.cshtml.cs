using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Authorization;
using Project.Services;

namespace Project.Pages.Admin.Identity;

[RequirePermission(Places.Identity, Actions.List)]
public class IndexModel : CrudPageModel
{
    public IndexModel(AdminRules rules, UserManager<Models.Identity> users) : base(rules, users) { }
    public IList<Models.Identity> Identities { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Identities = await users.Users.ToListAsync();
    }
}

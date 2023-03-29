using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;

namespace Project.Pages.Admin.Permissions.Identity;

public class IndexModel : PageModel
{
    private readonly UserManager<Models.Identity> users;
    public IndexModel(UserManager<Models.Identity> users) => this.users = users;
    public IList<Models.Identity> Identities { get; set; } = default!;
    public async Task OnGetAsync()
    {
        Identities = await users.GetUsersInRoleAsync(DefaultRoles.Staff.ToString());
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;

namespace Project.Pages.Admin.Permissions.Identity;

public class DetailsModel : PageModel
{
    private readonly UserManager<Models.Identity> users;
    public DetailsModel(UserManager<Models.Identity> users) => this.users = users;
    public Models.Identity Identity { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var identity = await users.FindByIdAsync(id.ToString()!);
        if (identity is null)
        {
            return NotFound();
        }
        if (!await users.IsInRoleAsync(identity, DefaultRoles.Staff.ToString()))
        {
            return Content("User is not member of Staff, cant have permissions.");
        }
        Identity = identity;
        return Page();
    }
}

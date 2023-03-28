using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.Identity;

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
        var result = await users.FindByIdAsync(id!.ToString()!);
        if (result is null)
        {
            return NotFound();
        }
        else
        {
            Identity = result;
        }
        return Page();
    }
}

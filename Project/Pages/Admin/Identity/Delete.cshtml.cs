using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.Identity;

public class DeleteModel : PageModel
{
    private readonly UserManager<Models.Identity> users;
    public DeleteModel(UserManager<Models.Identity> users) => this.users = users;
    [BindProperty]
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
        Identity = result;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
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
        Identity = result;
        await users.DeleteAsync(result);
        return Content("User removed");
    }
}

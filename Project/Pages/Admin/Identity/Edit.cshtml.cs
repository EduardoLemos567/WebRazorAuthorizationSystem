using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.Identity;

public class EditModel : PageModel
{
    private readonly UserManager<Models.Identity> users;
    public EditModel(UserManager<Models.Identity> users) => this.users = users;
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
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (!users.Users.Any(user => user.Id == Identity.Id))
        {
            return NotFound();
        }
        var result = await users.UpdateAsync(Identity);
        if (!result.Succeeded)
        {
            return Content("Couldnt update the user");
        }
        return Content("User updated");
    }
}

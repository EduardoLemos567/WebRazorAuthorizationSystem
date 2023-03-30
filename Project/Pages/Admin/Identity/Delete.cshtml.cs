using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Project.Pages.Admin.Identity;

public class DeleteModel : CrudPageModel
{
    public DeleteModel(UserManager<Models.Identity> users) : base(users) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var user = await this.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        Identity = Models.SummaryIdentity.FromIdentity(user);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var user = await this.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        var deleteResult = await users.DeleteAsync(user);
        if (!deleteResult.Succeeded) { return Content("Could not delete user."); }
        return Content("User deleted");
    }
}

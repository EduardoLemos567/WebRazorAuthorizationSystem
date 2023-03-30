using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Project.Pages.Admin.Identity;

public class DetailsModel : CrudPageModel
{
    public DetailsModel(UserManager<Models.Identity> users) : base(users) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var user = await this.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        Identity = Models.SummaryIdentity.FromIdentity(user);
        return Page();
    }
}

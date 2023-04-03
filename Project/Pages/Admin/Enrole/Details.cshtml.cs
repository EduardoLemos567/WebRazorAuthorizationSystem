using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Project.Pages.Admin.Enrole;

public class DetailsModel : CrudPageModel
{
    public DetailsModel(UserManager<Models.Identity> users, RoleManager<Models.Role> roles) : base(users, roles){}
    public Models.SummaryIdentity Identity { get; set; } = default!;
    public IList<Models.Role> Roles { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        /*
         show user + all roles
        var user = await this.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        Identity = Models.SummaryIdentity.FromIdentity(user);*/
        return Page();
    }
}

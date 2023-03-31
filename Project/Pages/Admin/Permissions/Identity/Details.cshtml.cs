using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;

namespace Project.Pages.Admin.Permissions.Identity;

public class DetailsModel : CrudPageModel
{
    public DetailsModel(UserManager<Models.Identity> users, CachedDefaultData cachedData) : base(users, cachedData) { }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var identity = await TryFindUserAsync(id);
        if (identity is null) { return NotFound(); }
        if (!await CanModifyPermissionsAsync(identity)) { return NotAllowedModify(); }
        Identity = Models.SummaryIdentity.FromIdentity(identity);
        return Page();
    }    
}

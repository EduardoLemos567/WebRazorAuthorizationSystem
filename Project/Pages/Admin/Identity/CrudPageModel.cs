using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin.Identity;

public class CrudPageModel : PageModel
{
    protected readonly UserManager<Models.Identity> users;
    public CrudPageModel(UserManager<Models.Identity> users) => this.users = users;
    protected async Task<Models.Identity?> TryFindUserAsync(int? id)
    {
        if (id is null) { return null; }
        return await users.FindByIdAsync(id!.ToString()!);
    }
    protected bool CheckPasswordErrors(IdentityResult result, string passwordPropName)
    {
        var filterPasswordErrors = from e in result.Errors where e.Description.Contains("Password") select e.Description;
        if (filterPasswordErrors.Any())
        {
            ModelState.AddModelError(passwordPropName, string.Join(' ', filterPasswordErrors));
            return true;
        }
        return false;
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Services;

namespace Project.Pages.Admin.Identity;

public class CrudPageModel : PageModel
{
    protected readonly AdminRules rules;
    protected readonly UserManager<Models.Identity> users;
    public CrudPageModel(AdminRules rules, UserManager<Models.Identity> users)
    {
        this.rules = rules;
        this.users = users;
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

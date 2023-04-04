using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Services;

namespace Project.Pages.Admin.Enrole;

public class CrudPageModel : PageModel
{
    protected readonly AdminRules rules;
    protected readonly UserManager<Models.Identity> users;
    public CrudPageModel(AdminRules rules, UserManager<Models.Identity> users)
    {
        this.rules = rules;
        this.users = users;
    }
}

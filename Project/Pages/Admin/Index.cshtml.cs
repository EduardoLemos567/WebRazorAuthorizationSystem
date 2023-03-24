using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages.Admin;

[Authorize(Roles = "Staff")]
public class IndexModel : PageModel
{
    public void OnGet() { }
}

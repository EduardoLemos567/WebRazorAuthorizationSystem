using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;

namespace Project.Pages.Account;

[Authorize]
public class LogoutModel : PageModel
{
    private readonly SignInManager<Identity> signor;
    private readonly ILogger<LogoutModel> logger;
    public LogoutModel(SignInManager<Identity> signor, ILogger<LogoutModel> logger)
    {
        this.signor = signor;
        this.logger = logger;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogDebug("Logging out...");
        await signor.SignOutAsync();
        logger.LogDebug("Logged out, redirecting...");
        return RedirectToPage("/Account/Login");
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Permissions;

namespace Project.Pages;

[HasPermission(Places.Movie, Actions.Create)]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    public IndexModel(ILogger<IndexModel> logger) => this.logger = logger;
    public void OnGet()
    {

    }
}
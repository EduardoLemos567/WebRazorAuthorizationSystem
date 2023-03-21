using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger) => this._logger = logger;

    public void OnGet()
    {

    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;

namespace Project.Pages.Admin;

[RequireRole(DefaultRoles.Staff)]
public class IndexModel : PageModel { }

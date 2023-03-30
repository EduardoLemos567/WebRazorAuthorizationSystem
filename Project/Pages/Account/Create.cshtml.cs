using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Account;

public class CreateModel : PageModel
{
    private readonly UserManager<Models.Identity> users;
    public CreateModel(UserManager<Models.Identity> users) => this.users = users;
    [BindProperty]
    public Models.SummaryIdentity SummaryIdentity { get; set; } = default!;
    [BindProperty, Required, DataType(DataType.Password)]
    public string NewPassword { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var identity = new Models.Identity();
        SummaryIdentity.Update(identity);
        var creationResult = await users.CreateAsync(identity, NewPassword!);
        if (!creationResult.Succeeded)
        {
            var filterPasswordErrors = from e in creationResult.Errors where e.Description.Contains("Password") select e.Description;
            if (filterPasswordErrors.Any())
            {
                ModelState.AddModelError(nameof(NewPassword), string.Join(' ', filterPasswordErrors));
                return Page();
            }
            else
            {
                return Content("User creation failed");
            }
        }
        var addToRoleResult = await users.AddToRoleAsync(identity, DefaultRoles.User.ToString());
        if (!addToRoleResult.Succeeded)
        {
            return Content("User creation failed");
        }
        Response.Headers.Add("REFRESH", "5;URL=/Account/Login");
        return Content("New user created");
    }
}

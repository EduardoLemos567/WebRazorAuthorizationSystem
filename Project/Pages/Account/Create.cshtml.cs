using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Account;

public class CreateModel : PageModel
{
    private readonly UserManager<Identity> users;
    public CreateModel(UserManager<Identity> users) => this.users = users;
    [BindProperty]
    public Identity Identity { get; set; } = default!;
    [BindProperty, Required, DataType(DataType.Password)]
    public string? Password { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var creationResult = await users.CreateAsync(Identity, Password!);
        if (!creationResult.Succeeded)
        {
            var filterPasswordErrors = from e in creationResult.Errors where e.Description.Contains("Password") select e.Description;
            if (filterPasswordErrors.Any())
            {
                ModelState.AddModelError("Password", string.Join(' ', filterPasswordErrors));
                return Page();
            }
            else
            {
                return Content("User creation failed");
            }
        }
        var addToRoleResult = await users.AddToRoleAsync(Identity, DefaultRoles.User.ToString());
        if (!addToRoleResult.Succeeded)
        {
            return Content("User creation failed");
        }
        Response.Headers.Add("REFRESH", "5;URL=/");
        return Content("New user created");
    }
}

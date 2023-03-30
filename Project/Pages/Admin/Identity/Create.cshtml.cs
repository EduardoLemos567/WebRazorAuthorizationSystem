using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Admin.Identity;

public class CreateModel : CrudPageModel
{
    public CreateModel(UserManager<Models.Identity> users) : base(users) { }
    [BindProperty, Required, DataType(DataType.Password)]
    public string NewPassword { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var identity = new Models.Identity();
        Identity.Update(identity);
        var createResult = await users.CreateAsync(identity, NewPassword!);
        if (!createResult.Succeeded)
        {
            if (this.CheckPasswordErrors(createResult, nameof(NewPassword))) { return Page(); }
            return Content("User creation failed");
        }
        Response.Headers.Add("REFRESH", "5;URL=/");
        return Content("New user created");
    }

}

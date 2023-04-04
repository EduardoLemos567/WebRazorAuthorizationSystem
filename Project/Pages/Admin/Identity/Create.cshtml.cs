using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Authorization;
using Project.Services;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Admin.Identity;

[RequirePermission(Places.Identity, Actions.Create)]
public class CreateModel : CrudPageModel
{
    public CreateModel(AdminRules rules, UserManager<Models.Identity> users) : base(rules, users) { }
    [BindProperty]
    public Models.SummaryIdentity Identity { get; set; } = default!;
    [BindProperty, Required, DataType(DataType.Password)]
    public string NewPassword { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var user = new Models.Identity();
        Identity.Update(user);
        var createResult = await users.CreateAsync(user, NewPassword!);
        if (!createResult.Succeeded)
        {
            if (this.CheckPasswordErrors(createResult, nameof(NewPassword))) { return Page(); }
            return Content("User creation failed");
        }
        var addToRoleResult = await users.AddToRoleAsync(user, DefaultRoles.Staff);
        if (!addToRoleResult.Succeeded)
        {
            return Content("User creation failed");
        }
        Response.Headers.Add("REFRESH", "5;URL=/");
        return Content("New user created");
    }

}

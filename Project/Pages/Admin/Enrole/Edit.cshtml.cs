using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Admin.Enrole;

public class EditModel : CrudPageModel
{
    public EditModel(UserManager<Models.Identity> users, RoleManager<Models.Role> roles) : base(users, roles){}
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        return Content("User updated");
        // Show all roles, let we select any of them
    }
}

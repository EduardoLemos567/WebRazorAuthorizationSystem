using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages.Admin.Identity;

public class EditModel : CrudPageModel
{
    public EditModel(UserManager<Models.Identity> users) : base(users) { }
    [BindProperty]
    public Models.SummaryIdentity Identity { get; set; } = default!;
    [BindProperty, DataType(DataType.Password), Display(Name = "New password")]
    public string? NewPassword { get; set; }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var user = await this.TryFindUserAsync(id);
        if (user is null) { return NotFound(); }
        Identity = Models.SummaryIdentity.FromIdentity(user);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        // Find user
        var user = await this.TryFindUserAsync(Identity.Id);
        if (user is null) { return NotFound(); }
        // Change password
        if (!string.IsNullOrEmpty(NewPassword))
        {
            var token = await users.GeneratePasswordResetTokenAsync(user);
            var resetResult = await users.ResetPasswordAsync(user, token, NewPassword);
            if (!resetResult.Succeeded)
            {
                if (this.CheckPasswordErrors(resetResult, nameof(NewPassword))) { return Page(); }
                return Content("User update failed");
            }
        }
        // Update other details
        Identity.Update(user);
        var updateResult = await users.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Content("User update failed");
        }
        return RedirectToPage("./Index");
    }
}

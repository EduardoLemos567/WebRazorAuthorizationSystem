using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Authorization;
using Project.Models;

namespace Project.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<Identity> users;
        private readonly SignInManager<Identity> signor;
        private readonly ILogger<CreateModel> logger;
        public CreateModel(UserManager<Identity> users, SignInManager<Identity> signor, ILogger<CreateModel> logger)
        {
            this.users = users;
            this.signor = signor;
            this.logger = logger;
        }
        [BindProperty]
        public AccountCreationInfo AccountInfo { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                logger.LogDebug("Model is not valid, returning to same page");
                return Page();
            }
            var user = new Identity
            {
                UserName = AccountInfo.UserName,
                Alias = AccountInfo.Alias,
                PhoneNumber = AccountInfo.PhoneNumber,
                Email = AccountInfo.Email,
            };
            var creationResult = await users.CreateAsync(user, AccountInfo.Password!);
            if (!creationResult.Succeeded)
            {
                logger.LogDebug($"User '{user}' creation failed, reason: {string.Join(',', from p in creationResult.Errors select p.Description)}");
                return Content("User creation failed");
            }
            var addToRoleResult = await users.AddToRoleAsync(user, DefaultRoles.User.ToString());
            if (!addToRoleResult.Succeeded)
            {
                logger.LogDebug($"User '{user}' could not be added to role {DefaultRoles.User}, reason: {string.Join(',', from p in addToRoleResult.Errors select p.Description)}");
                // TODO: either delete the user and return error or move on and mark to be added later by a background service
                // Right now, we do nothing.
            }
            var loginResult = await signor.PasswordSignInAsync(user, AccountInfo.Password!, false, false);
            if (!loginResult.Succeeded)
            {
                logger.LogDebug($"User '{user}' login failed, reason: {loginResult}");
                return RedirectToPage("./Account/Login");
            }
            return RedirectToPage("./Index");
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project.Models;
using Project.Data;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace Project.Login;

public interface IUserManager
{

}

public class AuthenticationService<TUser> where TUser : IdentityUser<int>
{
    public class LoginInfo
    {
        [Required]
        public string? User { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
    private readonly SignInManager<TUser> signInManager;
    public async Task<IdentityResult> TryCreateAccount(LoginInfo login)
    {

        if (!Validator.TryValidateObject(login, new ValidationContext(login), null))
        {
            // login invalid
        }
        var user = Activator.CreateInstance<TUser>();
        user.UserName = login.User;
        return await signInManager.UserManager.CreateAsync(user, login.Password!);
    }
    public async Task<IdentityResult> TrySignInAccount(LoginInfo login)
    {
        if (!Validator.TryValidateObject(login, new ValidationContext(login), null))
        {
            // login invalid
        }
        var result = await signInManager.PasswordSignInAsync(login.User!, login.Password!, login.RememberMe, true);
        if (result.Succeeded)
        {
            var user = await signInManager.UserManager.FindByNameAsync(login.User!);
            await signInManager.SignInWithClaimsAsync(user!, null, null);
        }
    }
    public async Task DeleteAccount()
    {

    }
    public async Task ResetPassword()
    {

    }
    public async Task ChangePassword()
    {

    }
}

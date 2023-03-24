using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project.Login;

public class AuthenticationService<TUser> where TUser : AAccount
{
    private const bool LOCKOUT_ON_FAILURE = true;
    public class LoginDetails
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
    private readonly UserManager<TUser> userManager;
    private readonly SignInManager<TUser> signInManager;
    private readonly ILogger<AuthenticationService<TUser>> logger;
    public AuthenticationService(UserManager<TUser> userManager, SignInManager<TUser> signInManager, ILogger<AuthenticationService<TUser>> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.logger = logger;
    }
    public async Task<IdentityResult> TryCreateAccount(TUser user, string password, PageModel page)
    {
        var creationResult = await userManager.CreateAsync(user, password);
        if (creationResult.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = page.Url.Action(new() { Action = "ConfirmEmail", Values = new { user.Email, token } });
            if (string.IsNullOrEmpty(url))
            {
                return CouldNotGenerateUrl();
            }
            await SendConfirmationEmail(user.Email!, url!);
        }
        return creationResult;
    }
    public Task<IdentityResult> TryCreateConfirmedAccount(TUser user, string password)
    {
        user.EmailConfirmed = true;
        return userManager.CreateAsync(user, password);
    }
    public async Task<SignInResult> TrySignInAccount(LoginDetails loginDetails)
    {
        if (!IsValid(loginDetails))
        {
            // Login is incomplete
        }
        var user = await userManager.FindByEmailAsync(loginDetails.Email!);
        if (user is null)
        {
            // No user with that email
        }
        var signInResult = await signInManager.CheckPasswordSignInAsync(user!, loginDetails.Password!, LOCKOUT_ON_FAILURE);
        if (!signInResult.Succeeded)
        {
            // Login did not succeed
        }
        // NOTE: need to change flow to enable two factor auth.
        if (user is StaffAccount staff)
        {
            await signInManager.SignInWithClaimsAsync(user!, loginDetails.RememberMe, new Claim[] { new("p", staff.Permissions!) });
        }
        else
        {
            await signInManager.SignInAsync(user!, loginDetails.RememberMe);
        }
        return signInResult;
    }
    public async Task<IdentityResult> ConfirmEmail(string email, string token)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return UserNotFound();
        }
        return await userManager.ConfirmEmailAsync(user!, token);
    }
    public async Task<IdentityResult> DeleteAccount(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return UserNotFound();
        }
        return await userManager.DeleteAsync(user!);
    }
    public async Task<IdentityResult> RequestResetPassword(string email, PageModel page)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return UserNotFound();
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var url = page.Url.Action(new() { Action = "RequestedResetPassword", Values = new { user.Email, token } });
        if (string.IsNullOrEmpty(url))
        {
            return CouldNotGenerateUrl();
        }
        await SendPasswordResetEmail(user.Email!, url!);
        return IdentityResult.Success;
    }
    public async Task<IdentityResult> ResetPassword(string email, string token, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return UserNotFound();
        }
        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }
    public async Task<IdentityResult> ChangePassword(string email, string currentPassword, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return UserNotFound();
        }
        return await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
    private async Task SendConfirmationEmail(string email, string url)
    {
        //TODO: send email
        logger.LogDebug($"confirmation email sent with url '{url}'");
        await Task.Delay(1);
    }
    private async Task SendPasswordResetEmail(string email, string url)
    {
        //TODO: send email
        logger.LogDebug($"password reset email sent with url '{url}'");
        await Task.Delay(1);
    }
    private IdentityResult UserNotFound()
    {
        logger.LogDebug($"User not found by email");
        return IdentityResult.Failed(new IdentityError() { Description = "User not found" });
    }
    private IdentityResult CouldNotGenerateUrl()
    {
        logger.LogDebug($"Could not generate token url");
        return IdentityResult.Failed(new IdentityError() { Description = "Could not generate token url" });
    }
    private static bool IsValid(LoginDetails login)
    {
        return !Validator.TryValidateObject(login, new ValidationContext(login), null);
    }
    private static void ThrowIfNull(in TUser? user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
    }

}

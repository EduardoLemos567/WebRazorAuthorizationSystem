using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Project.Models;

namespace Project.Services;

public class AuthenticationService
{
    public class NewUserInfo
    {
        public string? Alias { get; set; } = default!;
        public string? RealName { get; set; } = default!;
        public string? Email { get; set; } = default!;
        public string? Password { get; set; } = default!;
    }
    private readonly SignInManager<Identity> signor;
    private readonly UserManager<Identity> users;
    private readonly ILogger<AuthenticationService> logger;
    private readonly CookieAuthenticationOptions options;
    public Task SignOutAsync(Identity user)
    {
        return Task.CompletedTask;
    }
    public Task<SignInResult> TrySignInAsync(LoginInfo loginInfo)
    {
        throw new NotImplementedException();
    }
    public Task<IdentityResult> TryCreateNewUserAsync(NewUserInfo newUserInfo, params string[] roles)
    {
        throw new NotImplementedException();
    }
    public Task<IdentityResult> TryDeleteUserAsync(Identity user)
    {
        throw new NotImplementedException();
    }
    public Task TryRequestEmailConfirmationAsync()
    {
        throw new NotImplementedException();
    }
    public Task TryConfirmEmailAsync()
    {
        throw new NotImplementedException();
    }
    public Task TryResetPasswordAsync()
    {
        throw new NotImplementedException();
    }
    public Task TryRequestResetPasswordAsync()
    {
        throw new NotImplementedException();
    }
}

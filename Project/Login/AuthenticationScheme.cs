using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Project.Login;

public class AuthenticationScheme : CookieAuthenticationHandler
{
    public AuthenticationScheme(IOptionsMonitor<CookieAuthenticationOptions> options,
                                ILoggerFactory logger,
                                UrlEncoder encoder,
                                ISystemClock clock) : base(options, logger, encoder, clock)
    { }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await base.HandleAuthenticateAsync();
        if (result.Succeeded)
        {
            return result;
        }
        var claims = new List<Claim>
        {
            new("key", "value")
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "user"));
        await Context.SignInAsync("user", user);

        var a = new IdentityUser();

        return AuthenticateResult.Success(new(user, "user"));
    }
}

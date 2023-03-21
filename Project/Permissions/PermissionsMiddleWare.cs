using Microsoft.AspNetCore.DataProtection;

namespace Project.Permissions;

public class PermissionsMiddleWare
{
    private readonly RequestDelegate next;
    public PermissionsMiddleWare(RequestDelegate next) => this.next = next;
    public Task InvokeAsync(
        HttpContext context,
        ILogger<PermissionsMiddleWare> logger,
        IDataProtectionProvider protection
        )
    {
        const string payloadInitial = "eduardo";
        var protector = protection.CreateProtector("ourMethod");
        var cookies = context.Request.Cookies;
        var foundPayload = false;
        foreach (var pair in cookies)
        {
            try
            {
                var payload = protector.Unprotect(pair.Value);
                logger.LogDebug($"read cookie key: {pair.Key}, value: {payload}, original: {pair.Value}");
                foundPayload = true;
            }
            catch
            {
                logger.LogDebug($"read cookie, key: {pair.Key}, original: {pair.Value}, couldn't decrypt");
            }
        }
        if (!foundPayload)
        {
            var payload = protector.Protect(payloadInitial);
            context.Response.Cookies.Append("auth", payload);
            logger.LogDebug($"write cookie key: auth, value: {payload}, original: {payloadInitial}");
        }
        var endpoint = context.GetEndpoint();
        if (endpoint is not null)
        {
            logger.LogDebug(GetRequiredPermission(endpoint));
        }
        return next(context);
    }
    private string GetRequiredPermission(Endpoint endpoint)
    {
        var permissions = endpoint.Metadata.OfType<HasPermissionAttribute>();
        if (permissions.Any())
        {
            return permissions.First().permission.ToString();
        }
        return "NotRequired";
    }
}
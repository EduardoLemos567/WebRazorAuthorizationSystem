using Microsoft.AspNetCore.DataProtection;

namespace Project.Permissions;

public class PermissionsMiddleWare
{
    private readonly ILogger<PermissionsMiddleWare> logger;
    private readonly RequestDelegate next;
    private readonly PermissionService permissor;
    public PermissionsMiddleWare(RequestDelegate next,
                                 ILogger<PermissionsMiddleWare> logger,
                                 PermissionService permissor)
    {
        this.next = next;
        this.logger = logger;
        this.permissor = permissor;
    }
    public Task InvokeAsync(
        HttpContext context,
        IDataProtectionProvider protection)
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
            logger.LogDebug($"endpoint: '{endpoint}', permission: {permissor.GetPermission(endpoint)}");
        }
        return next(context);
    }
}

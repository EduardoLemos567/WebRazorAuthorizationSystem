using Microsoft.AspNetCore.DataProtection;

namespace Project.Permissions;

public class PermissionsMiddleWare
{
    private readonly RequestDelegate next;
    private readonly IReadOnlyDictionary<string, Permission> permissions;
    private readonly ILogger<PermissionsMiddleWare> logger;
    public PermissionsMiddleWare(RequestDelegate next,
                                 IEnumerable<EndpointDataSource> endpointSources,
                                 ILogger<PermissionsMiddleWare> logger)
    {
        this.next = next;
        permissions = BuildPermissions(endpointSources, logger);
        this.logger = logger;
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
            logger.LogDebug($"endpoint: '{endpoint}'");
            logger.LogDebug(GetRequiredPermission(endpoint));
        }
        return next(context);
    }
    private string GetRequiredPermission(Endpoint endpoint)
    {
        var route = endpoint.ToString();
        if (!string.IsNullOrEmpty(route) && permissions.TryGetValue(route, out var permission))
        {
            return permission.ToString();
        }
        return "NotRequired";
    }
    private static Dictionary<string, Permission> BuildPermissions(
        IEnumerable<EndpointDataSource> endpointSources,
        ILogger<PermissionsMiddleWare> logger)
    {
        var allEndpoints = from endpointSource in endpointSources from endpoint in endpointSource.Endpoints select endpoint;
        var permissions = new Dictionary<string, Permission>();
        foreach (var endpoint in allEndpoints)
        {
            foreach (var meta in endpoint.Metadata)
            {
                if (meta is HasPermissionAttribute hasPermission)
                {
                    // In the list of endpoints, each Index PageModel is duplicated for some reason.
                    // So we add only once.
                    permissions.TryAdd(endpoint.DisplayName!, hasPermission.permission);
                }
            }
        }
        logger.LogDebug($"All permissions: {string.Join(',', permissions.Keys)}");
        return permissions;
    }
}
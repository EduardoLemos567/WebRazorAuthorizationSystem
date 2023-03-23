using Project.Models;

namespace Project.Permissions;

public class PermissionService
{
    private readonly ILogger<PermissionService> logger;
    private readonly IReadOnlyDictionary<string, Permission> permissions;
    public IEnumerable<Permission> AllPermissions => permissions.Values;
    public PermissionService(ILogger<PermissionService> logger,
                             IEnumerable<EndpointDataSource> endpointSources)
    {
        this.logger = logger;   // Logger must be set before everything
        permissions = BuildPermissions(endpointSources);
    }
    public bool HasPermission(in StaffAccount staff, in Endpoint endpoint)
    {
        var route = endpoint.ToString();
        if (!string.IsNullOrEmpty(route) && permissions.TryGetValue(route, out var permission))
        {
            return staff.HasPermission(permission);
        }
        return true;
    }
    public Permission GetPermission(in Endpoint endpoint)
    {
        if (string.IsNullOrEmpty(endpoint.DisplayName)
            && permissions.TryGetValue(endpoint.DisplayName!, out var permission))
        {
            return permission;
        }
        return Permission.NOT_REQUIRED;
    }
    private Dictionary<string, Permission> BuildPermissions(
        IEnumerable<EndpointDataSource> endpointSources)
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
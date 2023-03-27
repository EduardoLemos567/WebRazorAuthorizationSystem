using System.Reflection;

namespace Project.Authorization;

public static class Requirements
{
    public const string PERMISSIONS_CLAIM_TYPE = "Permissions";
    public static IEnumerable<Permission> AllRequiredPermissions()
    {
        var a = Assembly.GetExecutingAssembly();
        return (from t in a.GetTypes()
                from c in t.GetCustomAttributes<RequirePermissionAttribute>()
                select c.permission).Distinct();
    }
    public static string AllRequiredPermissionsString()
    {
        return new((from c in AllRequiredPermissions() select c.data).ToArray());
    }
    public static IEnumerable<DefaultRoles> AllRequiredRoles()
    {
        var a = Assembly.GetExecutingAssembly();
        return (from t in a.GetTypes()
                from c in t.GetCustomAttributes<RequireRoleAttribute>()
                from r in c.roles
                select r).Distinct();
    }
}
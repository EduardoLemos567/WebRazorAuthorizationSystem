using System.Reflection;

namespace Project.Requirements;

public enum Places : short
{
    NoPlace,
    Movie,
    MovieCategory,
    UserAccount,
    StaffAccount,
    StaffRole,
    // Max 8191 options = 13 bits
}

public enum Actions : short
{
    NoAction,
    Create,
    Read,   // Read individual details
    Update,
    Delete,
    List,   // List page
    // Max 8 options = 3 bits
}

public enum DefaultRoles
{
    User,
    Staff,
    Admin,
}

public static class Requirements
{
    public const string PERMISSIONS_CLAIM_TYPE = "Permissions";
    public static IEnumerable<Permission> AllRequiredPermissions()
    {
        return (from a in AppDomain.CurrentDomain.GetAssemblies()
                from c in a.GetCustomAttributes<RequirePermissionAttribute>()
                select c.permission).Distinct();
    }
    public static string AllRequiredPermissionsString()
    {
        return new((from c in AllRequiredPermissions() select c.data).ToArray());
    }
    public static IEnumerable<DefaultRoles> AllRequiredRoles()
    {
        return (from a in AppDomain.CurrentDomain.GetAssemblies()
                from c in a.GetCustomAttributes<RequireRoleAttribute>()
                from r in c.roles
                select r).Distinct();
    }
}
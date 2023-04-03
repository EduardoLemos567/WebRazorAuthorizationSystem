using System.Reflection;
using System.Security.Claims;

namespace Project.Authorization;

//TODO: find a better name, "Utils"
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
    public static string PermissionsIntoString(IEnumerable<Permission> permissions)
    {
        return new((from p in permissions select p.data).ToArray());
    }
    public static IEnumerable<Permission> PermissionsFromString(string permissions)
    {
        foreach (var c in permissions) { yield return new(c); }
    }
    public static Claim PermissionsIntoClaim(IEnumerable<Permission> permissions)
    {
        return new(PERMISSIONS_CLAIM_TYPE, PermissionsIntoString(permissions));
    }
    public static IList<int> PermissionsStringToIndices(in string stringPermissions, IReadOnlyList<Permission> sortedPermissions)
    {
        var permissionsChars = stringPermissions.ToArray();
        Array.Sort(permissionsChars);
        var selected = new int[permissionsChars.Length];
        var selectedCount = 0;
        for (int i = 0; i < sortedPermissions.Count; i++)
        {
            if (sortedPermissions[i].data == permissionsChars[selectedCount])
            {
                selected[selectedCount++] = i;
                if (selectedCount >= selected.Length) { break; }
            }
        }
        return selected;
    }
    public static string PermissionsIndicesToString(in IList<int> permissionsIndices, IReadOnlyList<Permission> sortedPermissions)
    {
        var permissionsChars = new char[permissionsIndices.Count];
        for (int i = 0; i < permissionsIndices.Count; i++)
        {
            permissionsChars[i] = sortedPermissions[permissionsIndices[i]].data;
        }
        return new(permissionsChars);
    }
}
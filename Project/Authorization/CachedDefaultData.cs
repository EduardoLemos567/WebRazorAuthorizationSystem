namespace Project.Authorization;

// Need a better name
public class CachedDefaultData
{
    public IReadOnlyList<Permission> SortedPermissions { get; }
    public IReadOnlyList<string> SortedPermissionsStrings { get; }
    public IReadOnlyList<string> SortedDefaultRoles { get; }
    public CachedDefaultData()
    {
        var sortedPermissions = Requirements.AllRequiredPermissions().ToArray();
        Array.Sort(sortedPermissions, (a, b) => a.data.CompareTo(b.data));
        SortedPermissions = sortedPermissions;
        SortedPermissionsStrings = sortedPermissions.Select((e) => e.ToString()).ToArray();
        var sortedRoles = Enum.GetNames<DefaultRoles>();
        Array.Sort(sortedRoles);
        SortedDefaultRoles = sortedRoles;
    }
    public bool IsDefaultRole(Models.Role role) => SortedDefaultRoles.Contains(role.Name);
}

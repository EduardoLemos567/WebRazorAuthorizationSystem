namespace Project.Authorization;

public class CachedPermissions
{
    public IReadOnlyList<Permission> SortedPermissions { get; }
    public IReadOnlyList<string> SortedPermissionsStrings { get; }
    public CachedPermissions()
    {
        var sortedPermissions = Requirements.AllRequiredPermissions().ToArray();
        Array.Sort(sortedPermissions, (a, b) => a.data.CompareTo(b.data));
        SortedPermissions = sortedPermissions;
        SortedPermissionsStrings = sortedPermissions.Select((e) => e.ToString()).ToArray();
    }
}

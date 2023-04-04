namespace Project.Authorization;

public enum Places : short
{
    NoPlace,
    Movie,
    MovieCategory,
    Identity,
    Role,
    PermissionsIdentity,
    PermissionsRole,
    Enrole,
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

public static class DefaultRoles
{
    public const string User = "User";
    public const string Staff = "Staff";
    public const string Admin = "Admin";
    public static bool IsDefaultRole(string name)
    {
        return name is User
            or Staff
            or Admin;
    }
    public static IEnumerable<string> GetEnumerable()
    {
        yield return User;
        yield return Staff;
        yield return Admin;
    }
}
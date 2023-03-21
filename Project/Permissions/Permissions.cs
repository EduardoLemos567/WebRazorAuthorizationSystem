namespace Project.Permissions;

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
    Read,
    Update,
    Delete,
    // Max 8 options = 3 bits
}
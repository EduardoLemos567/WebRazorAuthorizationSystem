using Microsoft.AspNetCore.Identity;

namespace Project.Models;

public class Identity : IdentityUser<int>
{
    [ProtectedPersonalData]
    public string? Alias { get; set; }
    public Identity() { }
    public Identity(string userName) : base(userName) { }
}

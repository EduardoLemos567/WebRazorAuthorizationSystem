using Microsoft.AspNetCore.Identity;

namespace Project.Models;

public class Role : IdentityRole<int>
{
    public Role() { }
    public Role(string roleName) : base(roleName) { }
}

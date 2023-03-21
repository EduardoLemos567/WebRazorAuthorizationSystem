using Microsoft.AspNetCore.Identity;
using Project.Permissions;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class StaffAccount : Account
{
    [Required]
    public string? Permissions { get; set; }
    public IEnumerable<Permission> PermissionsList => from c in Permissions! select new Permission(c);
    public void SortPermissions()
    {
        var charArray = Permissions!.ToCharArray();
        Array.Sort(charArray);
        Permissions = new(charArray);
    }
    public bool HasPermission(in Permission permission)
    {
        foreach (var c in Permissions!)
        {
            if (permission.data == c)
            {
                return true;
            }
        }
        return false;
    }
}
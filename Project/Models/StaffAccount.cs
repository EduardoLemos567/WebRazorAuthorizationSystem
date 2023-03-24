using Project.Permissions;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class StaffAccount : AAccount
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
    public bool HasPermission(in Permission permission) => Permissions!.Contains(permission.data);
}
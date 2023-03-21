using Project.Permissions;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public class PermissionsPackage
{
    [Required, Key]
    public int Id { get; set; }
    [Required, StringLength(100, MinimumLength = 3)]
    public string? Name { get; set; }
    [Required]
    public string? Permissions { get; set; }
    public IEnumerable<Permission> PermissionsList => from c in Permissions! select new Permission(c);
    public void SortPermissions()
    {
        var charArray = Permissions!.ToCharArray();
        Array.Sort(charArray);
        Permissions = new(charArray);
    }
    public void ApplyInto(in StaffAccount account)
    {
        //TODO:
    }
}

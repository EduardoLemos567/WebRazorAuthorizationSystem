using Microsoft.AspNetCore.Authorization;

namespace Project.Permissions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute : Attribute
{
    public readonly Permission permission;
    public HasPermissionAttribute(Places place, Actions action) => permission = new(place, action);
}

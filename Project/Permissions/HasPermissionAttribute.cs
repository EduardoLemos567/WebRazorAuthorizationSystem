namespace Project.Permissions;

[AttributeUsage(AttributeTargets.Class)]
public class HasPermissionAttribute : Attribute
{
    public readonly Permission permission;
    public HasPermissionAttribute(Places place, Actions action) => permission = new(place, action);
    public override string ToString() => permission.ToString();
}

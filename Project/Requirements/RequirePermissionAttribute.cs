using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project.Requirements;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    public readonly Permission permission;
    public RequirePermissionAttribute(Places place, Actions action) => permission = new(place, action);
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var q = from c in context.HttpContext.User.Claims where c.Type == Requirements.PERMISSIONS_CLAIM_TYPE select c.Value;
        foreach (var s in q)
        {
            if (s.Contains(permission.data))
            {
                return;
            }
        }
        context.Result = new ForbidResult();
    }
    public override string ToString() => $"Require permission: {permission}";
}

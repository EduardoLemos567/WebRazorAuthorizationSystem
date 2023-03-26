using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project.Requirements;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRoleAttribute : Attribute, IAuthorizationFilter
{
    public readonly DefaultRoles[] roles;
    public RequireRoleAttribute(params DefaultRoles[] roles) => this.roles = roles;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        foreach (var r in roles)
        {
            if (!context.HttpContext.User.IsInRole(r.ToString()))
            {
                context.Result = new ForbidResult();
            }
        }
    }
    public override string ToString() => $"Require roles: {string.Join(',', roles)}";
}

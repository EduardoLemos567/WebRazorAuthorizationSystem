using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRoleAttribute : Attribute, IAuthorizationFilter
{
    public readonly string[] roles;
    public RequireRoleAttribute(params string[] roles) => this.roles = roles;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        foreach (var r in roles)
        {
            if (!context.HttpContext.User.IsInRole(r))
            {
                context.Result = new ForbidResult();
            }
        }
    }
    public override string ToString() => $"Require roles: {string.Join(',', roles)}";
}

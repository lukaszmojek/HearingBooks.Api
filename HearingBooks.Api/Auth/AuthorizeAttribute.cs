using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#pragma warning disable CS1591

namespace HearingBooks.Api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.User();

        if (user == null)
            context.Result = new JsonResult(new {message = "Unauthorized"})
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
    }
}
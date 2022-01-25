using HearingBooks.Domain;
using HearingBooks.Domain.ValueObjects;
using HearingBooks.Domain.ValueObjects.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#pragma warning disable CS1591

namespace HearingBooks.Api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IsAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.User();

        if (user?.Type != UserType.HearingBooks)
            context.Result = new JsonResult(new {message = "Forbidden"})
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
    }
}
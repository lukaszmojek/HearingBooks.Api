using System;
using EasySynthesis.Domain.ValueObjects.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#pragma warning disable CS1591

namespace EasySynthesis.Api.Core.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IsAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.User();

        if (user?.Type != UserType.EasySynthesis)
            context.Result = new JsonResult(new {message = "Forbidden"})
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
    }
}
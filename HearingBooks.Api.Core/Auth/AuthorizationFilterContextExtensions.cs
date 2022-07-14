using EasySynthesis.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasySynthesis.Api.Core.Auth;

public static class AuthorizationFilterContextExtensions
{
    public static User User(this AuthorizationFilterContext context)
    {
        return (User) context.HttpContext.Items["User"];
    }
}
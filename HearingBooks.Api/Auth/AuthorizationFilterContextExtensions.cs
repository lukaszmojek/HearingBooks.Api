using HearingBooks.Domain;
using HearingBooks.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HearingBooks.Api.Auth;

public static class AuthorizationFilterContextExtensions
{
    public static User User(this AuthorizationFilterContext context)
    {
        return (User) context.HttpContext.Items["User"];
    }
}
using HearingBooks.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HearingBooks.Api.Core.Auth;

public static class AuthorizationFilterContextExtensions
{
    public static User User(this AuthorizationFilterContext context) => (User) context.HttpContext.Items["User"];
}
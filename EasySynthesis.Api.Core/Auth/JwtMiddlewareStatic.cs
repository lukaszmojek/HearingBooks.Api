using System;

#pragma warning disable CS1591
namespace EasySynthesis.Api.Core.Auth;

public partial class JwtMiddleware
{
    public static void LogDetails(Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
using System;

namespace EasySynthesis.Api.Core.Auth;

public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message)
    {
    }
}
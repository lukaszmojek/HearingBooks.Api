using System;

namespace HearingBooks.Api.Core.Auth;

public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message)
    {
    }
}
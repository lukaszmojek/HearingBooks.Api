namespace HearingBooks.Api.Auth;

public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message)
    {
    }
}
namespace Contracts.Responses
{
    public class AuthenticateUserResponse : IResponse
    {
        public string Token { get; set; }
    }
}
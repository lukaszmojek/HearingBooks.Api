using Contracts.Responses;

namespace HearingBooks.Api.Auth.Login
{
    public class LoginUserResponse : IResponse
    {
        public string Token { get; set; }
    }
}
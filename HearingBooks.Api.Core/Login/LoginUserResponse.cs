using HearingBooks.Api.Core.Responses;

namespace HearingBooks.Api.Core.Login
{
    public class LoginUserResponse : IResponse
    {
        public string Token { get; set; }
    }
}
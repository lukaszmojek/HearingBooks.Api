using EasySynthesis.Api.Core.Responses;

namespace EasySynthesis.Api.Core.Login
{
    public class LoginUserResponse : IResponse
    {
        public string Token { get; set; }
    }
}
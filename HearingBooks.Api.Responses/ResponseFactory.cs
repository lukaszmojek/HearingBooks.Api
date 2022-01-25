using Contracts.Responses;

namespace Api.Factories
{
    public static class ResponseFactory
    {
        public static Response<T> CreateSuccessResponse<T>(T content = default) where T : IResponse, new() 
        {
            return new Response<T>
            {
                Succeeded = true,
                Error = null,
                Content = content
            };
        }
        
        public static Response<T> CreateFailureResponse<T>(string errorMessage = "") where T : IResponse, new() 
        {
            return new Response<T>
            {
                Succeeded = false,
                Error = errorMessage
            };
        }
    }
}
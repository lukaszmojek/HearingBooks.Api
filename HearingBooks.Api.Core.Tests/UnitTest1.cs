using HearingBooks.Api.Core.Auth;
using HearingBooks.Api.Core.Configuration;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Tests.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HearingBooks.Api.Core.Tests;

public class JwtMiddlewareTests
{
    private readonly Mock<RequestDelegate> _requestDelegateMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<HttpContext> _httpContextMock;
    private readonly IApiConfiguration _configuration;
    private readonly JwtMiddleware _jwtMiddleware;
    
    public JwtMiddlewareTests()
    {
        _requestDelegateMock = new Mock<RequestDelegate>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _httpContextMock = new Mock<HttpContext>();
        _configuration = new ApiConfiguration(
            ConfigurationHelpers.CreateConfiguration());

        _jwtMiddleware = new JwtMiddleware(_requestDelegateMock.Object, _configuration);
    }
    
    // [Fact]
    // public void Should_Not_Attach_User_To_Context_When_Authorization_Header_Is_Missing()
    // {
    //     _httpContextMock.SetupGet(x => x.Request.Headers)
    //         .Returns(new Dictionary<string, string>()
    //         {
    //             {"d", "sasdas"}
    //         });
    // }
}
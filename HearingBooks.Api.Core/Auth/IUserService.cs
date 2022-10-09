using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Core.Auth;

public interface IUserService
{
    string Authenticate(User user);
}
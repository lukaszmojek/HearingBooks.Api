using HearingBooks.Domain;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Auth;

public interface IUserService
{
    string Authenticate(User user);
}
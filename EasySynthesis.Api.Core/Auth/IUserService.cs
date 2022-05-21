using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Api.Core.Auth;

public interface IUserService
{
    string Authenticate(User user);
}
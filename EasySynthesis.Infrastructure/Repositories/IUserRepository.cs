using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByCredentialsAsync(string email, string password);
}
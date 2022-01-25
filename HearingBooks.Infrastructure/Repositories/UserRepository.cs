using HearingBooks.Domain.Entities;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HearingBooksDbContext _dbContext;
    private readonly DbSet<User> _dbSet;
    
    public UserRepository(HearingBooksDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<User>();
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var user = _dbSet.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            throw new Exception("User does not exist!");
        }

        return user;
    }

    public async Task<User> GetUserByCredentialsAsync(string email, string password)
    {
        var user = await _dbSet.SingleOrDefaultAsync(x => 
            x.Email == email && x.Password == password);

        return user;
    }
}
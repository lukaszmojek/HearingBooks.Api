using System.Security.Cryptography;
using System.Text;
using EasySynthesis.Domain.Entities;
using EasySynthesis.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EasySynthesis.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HearingBooksDbContext _dbContext;
    private readonly DbSet<User> _dbSet;
    
    public UserRepository(HearingBooksDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<User>();
    }

    public async Task AddAsync(User user)
    {
        user.PasswordHash = CalculateHash(user.Password);
        await _dbSet.AddAsync(user);
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var user = _dbSet
            .Include(x => x.Preference)
            .FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            throw new Exception("User does not exist!");
        }

        return user;
    }

    public async Task<User> GetUserByCredentialsAsync(string email, string password)
    {
        var passwordHash = CalculateHash(password);
        
        var user = await _dbSet.SingleOrDefaultAsync(x => 
            x.Email == email && x.PasswordHash == passwordHash);

        return user;
    }
    
    static string CalculateHash(string password)
    {
        var passwordBytes = Encoding.ASCII.GetBytes(password);
        var passwordHash = ByteArrayToString(SHA512.Create().ComputeHash(passwordBytes));

        return passwordHash;
    }
    
    static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (i=0;i < arrInput.Length; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }

    public async Task TopUpAsync(Guid userId, int amount)
    {
        var user = _dbSet
            .Include(x => x.Preference)
            .FirstOrDefault(x => x.Id == userId);

        user.Balance += amount;

        await _dbContext.SaveChangesAsync();
    }
}
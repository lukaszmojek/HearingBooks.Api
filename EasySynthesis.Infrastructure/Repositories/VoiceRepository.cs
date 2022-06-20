using EasySynthesis.Domain.Entities;
using EasySynthesis.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EasySynthesis.Infrastructure.Repositories;

public class VoiceRepository : IVoiceRepository
{
    private HearingBooksDbContext _context { get; set; }
    private DbSet<Voice> _dbset { get; set; }
	
    public VoiceRepository(HearingBooksDbContext context)
    {
        _context = context;
        _dbset = _context.Set<Voice>();
    }

    public async Task<Voice> GetVoiceByName(string name)
    {
        return await _dbset.FirstAsync(x => x.Name == name);
    }
}
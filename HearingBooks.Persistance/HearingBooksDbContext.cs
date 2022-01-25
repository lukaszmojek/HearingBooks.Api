using System.Reflection;
using HearingBooks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HearingBooks.Persistance;

public class HearingBooksDbContext : DbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<TextSynthesis> TextSyntheses { get; set; }
	public DbSet<Language> Languages { get; set; }
	public DbSet<Voice> Voices { get; set; }

	public HearingBooksDbContext(DbContextOptions<HearingBooksDbContext> options)
		: base(options)
	{
	}
}
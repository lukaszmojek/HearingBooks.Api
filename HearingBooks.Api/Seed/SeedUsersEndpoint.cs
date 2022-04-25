using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.User;
using HearingBooks.Persistance;

namespace HearingBooks.Api.Seed;

public class SeedUsersEndpoint : EndpointWithoutRequest
{
	private readonly HearingBooksDbContext _context;
	private readonly string _password = "Resubmit-Gas-Dreamland-Sizable-Relapsing-Sprinkled7-Debatable";

	public SeedUsersEndpoint(HearingBooksDbContext context)
	{
		_context = context;
	}

	public override void Configure()
	{
		Get("seed/users");
		Roles("HearingBooks");
	}
	
	public override async Task HandleAsync(CancellationToken ct)
	{
		var users = new List<User>
		{
		    new()
		    {
		        Id = Guid.NewGuid(),
		        FirstName = "Łukasz",
		        LastName = "Mojek",
		        UserName = "shaggy",
		        Email = "lukasz@hb.com",
		        Password = _password, 
		        EmailNotificationsEnabled = true,
		        EmailIsUsername = false,
		        IsActive = true,
		        Type = UserType.HearingBooks,
		    },
		    new()
		    {
		        Id = Guid.NewGuid(),
		        FirstName = "Łukasz",
		        LastName = "Mojek",
		        UserName = "user",
		        Email = "user@email.com",
		        Password = _password, 
		        EmailNotificationsEnabled = true,
		        EmailIsUsername = true,
		        IsActive = true,
		        Type = UserType.PayAsYouGo,
		    },
		    new()
		    {
			    Id = Guid.NewGuid(),
			    FirstName = "Darka",
			    LastName = "Koparka",
			    UserName = "darkaKoparka",
			    Email = "darka@email.com",
			    Password = _password, 
			    EmailNotificationsEnabled = true,
			    EmailIsUsername = true,
			    IsActive = true,
			    Type = UserType.PayAsYouGo,
		    }
		};
		
		var usersToDelete = _context.Users
		    .AsEnumerable()
		    .Where(entity => users.Any(user => user.Email == entity.Email));
		
		_context.Users.RemoveRange(usersToDelete);
		await _context.SaveChangesAsync();
		
		await _context.Users.AddRangeAsync(users);
		await _context.SaveChangesAsync();

		await SendOkAsync();
	}
}
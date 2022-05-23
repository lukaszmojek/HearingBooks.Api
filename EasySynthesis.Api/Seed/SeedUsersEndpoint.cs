using EasySynthesis.Domain.Entities;
using EasySynthesis.Domain.ValueObjects.User;
using EasySynthesis.Persistance;

namespace EasySynthesis.Api.Seed;

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
		        Id = SeedConfig.AdminId,
		        FirstName = "Łukasz",
		        LastName = "Mojek",
		        UserName = "shaggy",
		        Email = "lukasz@hb.com",
		        Password = _password, 
		        Preference = new Preference()
		        {
			        EmailNotificationsEnabled = false,
			        AcrylicEnabled = true,
			        Language = "pl"
		        },
		        IsActive = true,
		        Type = UserType.HearingBooks,
		        Balance = 0
		    },
		    new()
		    {
		        Id = SeedConfig.PayAsYouGoId,
		        FirstName = "Łukasz",
		        LastName = "Mojek",
		        UserName = "user",
		        Email = "user@email.com",
		        Password = _password,
		        Preference = new Preference()
		        {
			        EmailNotificationsEnabled = true,
					AcrylicEnabled = true,
					Language = "en"
		        },
		        IsActive = true,
		        Type = UserType.PayAsYouGo,
		        Balance = 100
		    },
		    new()
		    {
			    Id = SeedConfig.TestUserId,
			    FirstName = "Łukasz",
			    LastName = "Mojek",
			    UserName = "test user",
			    Email = "test@email.com",
			    Password = _password, 
			    Preference = new Preference()
			    {
				    EmailNotificationsEnabled = true,
				    AcrylicEnabled = true,
				    Language = "pl"
			    },
			    IsActive = true,
			    Type = UserType.PayAsYouGo,
			    Balance = 50
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
using HearingBooks.Domain.Entities;
using HearingBooks.Domain.ValueObjects.User;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;

namespace HearingBooks.Api.Seed;

public class SeedUsersEndpoint : EndpointWithoutRequest
{
	private readonly HearingBooksDbContext _context;
	private readonly IUserRepository _userRepository;
	private readonly string _password = "Resubmit-Gas-Dreamland-Sizable-Relapsing-Sprinkled7-Debatable";
	
	public SeedUsersEndpoint(HearingBooksDbContext context, IUserRepository userRepository)
	{
		_context = context;
		_userRepository = userRepository;
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
		        Email = "lukasz.mojek@gmail.com",
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
		    },
		    new()
		    {
			    Id = SeedConfig.TestHearingBooksId,
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
			    Type = UserType.HearingBooks,
			    Balance = 50
		    }
		};
		
		var usersToDelete = _context.Users
		    .AsEnumerable()
		    .Where(entity => users.Any(user => user.Id == entity.Id));
		
		_context.Users.RemoveRange(usersToDelete);
		await _context.SaveChangesAsync();
		
		// await _context.Users.AddRangeAsync(users);
		foreach (var user in users)
		{
			await _userRepository.AddAsync(user);
			await _context.SaveChangesAsync();
		}

		await SendOkAsync();
	}
}
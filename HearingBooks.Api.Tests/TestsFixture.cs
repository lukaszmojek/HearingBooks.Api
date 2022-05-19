using HearingBooks.Api.Auth;
using HearingBooks.Api.Configuration;
using HearingBooks.Api.Speech;
using HearingBooks.Api.Storage;
using HearingBooks.Api.Syntheses.TextSyntheses;
using HearingBooks.Infrastructure.Repositories;
using HearingBooks.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HearingBooks.Api.Tests;

public static class TestsFixture
{
	private static WebApplication _app;
	
	static TestsFixture()
	{
		CreateBulider();
	}
	
	public static void CreateBulider()
	{
		var builder = WebApplication.CreateBuilder();

		builder.Configuration
			.AddJsonFile("appsettings.json")
			.AddJsonFile("appsettings.Development.json");
		
		builder.Services.AddSingleton<IApiConfiguration, ApiConfiguration>();

		builder.Services.AddDbContext<HearingBooksDbContext>(
			options =>
			{
				options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"))
					.EnableSensitiveDataLogging();
			});
		
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<IStorageService, StorageService>();
		builder.Services.AddScoped<ISpeechService, SpeechService>();
		builder.Services.AddScoped<IFileService, FileService>();
		
		builder.Services.AddScoped<TextSynthesisService, TextSynthesisService>();
		
		builder.Services.AddScoped<IDialogueSynthesisRepository, DialogueSynthesisRepository>();
		builder.Services.AddScoped<IUserRepository, UserRepository>();

		_app = builder.Build();
	}

	public static HearingBooksDbContext GetDbContext()
	{
		var optionsBuilder = new DbContextOptionsBuilder<HearingBooksDbContext>();

		optionsBuilder.UseNpgsql(GetService<IApiConfiguration>()["ConnectionStrings:DatabaseUrl"]);

		return new HearingBooksDbContext(optionsBuilder.Options);
	}
	
	public static T? GetService<T>() => _app.Services.GetService<T>();
}
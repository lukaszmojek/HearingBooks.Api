using HearingBooks.Api.Core.Auth;
using HearingBooks.Api.Core.Configuration;
using HearingBooks.Infrastructure;
using HearingBooks.Persistance;
using HearingBooks.Services.Core.Storage;
using HearingBooks.SynthesisProcessor.Services;
using HearingBooks.SynthesisProcessor.Services.Speech;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HearingBooks.Tests.Core;

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
		builder.Services.AddScoped<ISynthesisPricingService, SynthesisPricingService>();
		
		builder.Services.AddScoped<TextSynthesisService, TextSynthesisService>();
		
		builder.Services.RegisterRepositories();

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
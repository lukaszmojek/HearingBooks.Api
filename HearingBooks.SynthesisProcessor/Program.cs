using HearingBooks.Api.Core.TimeProvider;
using HearingBooks.Common.Mapper;
using HearingBooks.Infrastructure;
using HearingBooks.Persistance;
using HearingBooks.Services.Core.Storage;
using HearingBooks.SynthesisProcessor;
using HearingBooks.SynthesisProcessor.Services;
using HearingBooks.SynthesisProcessor.Services.Speech;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args);

var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.AddJsonFile("appsettings.Development.json")
	.Build();

IHost host = builder
	.ConfigureServices(
		services =>
		{
			services.AddDbContext<HearingBooksDbContext>(
				options =>
			{
					options.UseNpgsql(configuration.GetConnectionString("DatabaseUrl"))
						.EnableSensitiveDataLogging();
				});
			
			services.RegisterRepositories();
			
			services.AddAutoMapper(typeof(TextSynthesisProfile));
			services.AddScoped<ITimeProvider, TimeProvider>();

			services.AddScoped<ISpeechService, SpeechService>();
			services.AddScoped<IStorageService, StorageService>();
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<ISynthesisPricingService, SynthesisPricingService>();

			services.AddScoped<TextSynthesisService>();
			services.AddScoped<DialogueSynthesisService>();

			services.AddMassTransit(x =>
			{
				// elided...
				x.SetKebabCaseEndpointNameFormatter();
				
				x.UsingRabbitMq((context,cfg) =>
				{
					cfg.Host("localhost", "/", h => {
						h.Username("guest");
						h.Password("guest");
					});

					cfg.ConfigureEndpoints(context);
				});
			
				x.AddConsumers(typeof(Worker).Assembly);
			
				// OPTIONAL, but can be used to configure the bus options
				services.AddOptions<MassTransitHostOptions>()
					.Configure(options =>
					{
						// if specified, waits until the bus is started before
						// returning from IHostedService.StartAsync
						// default is false
						options.WaitUntilStarted = true;
				
						// if specified, limits the wait time when starting the bus
						options.StartTimeout = TimeSpan.FromSeconds(10);
				
						// if specified, limits the wait time when stopping the bus
						options.StopTimeout = TimeSpan.FromSeconds(30);
					});
			});
			
			services.AddHostedService<Worker>();
		})
	.Build();

await host.RunAsync();
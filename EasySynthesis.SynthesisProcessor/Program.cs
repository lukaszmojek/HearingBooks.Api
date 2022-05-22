using EasySynthesis.SynthesisProcessor;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(
		services =>
		{
			services.AddHostedService<Worker>();
			
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
			
		})
	.Build();

await host.RunAsync();
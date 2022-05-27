using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EasySynthesis.MassTransit;

public static class IServiceCollectionExtensions
{
	public static void AddEasySynthesisMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(x =>
		{
			x.SetKebabCaseEndpointNameFormatter();

			x.UsingRabbitMq((context,cfg) =>
			{
				cfg.Host("localhost", "/", h => {
					h.Username("guest");
					h.Password("guest");
				});

				cfg.ConfigureEndpoints(context);
			});
			
			services.AddOptions<MassTransitHostOptions>()
				.Configure(options =>
				{
					options.WaitUntilStarted = true;
					options.StartTimeout = TimeSpan.FromSeconds(10);
					options.StopTimeout = TimeSpan.FromSeconds(30);
				});
		});
	}
}
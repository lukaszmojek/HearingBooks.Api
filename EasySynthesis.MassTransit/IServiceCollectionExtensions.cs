using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EasySynthesis.Communication;

public static class IServiceCollectionExtensions
{
	public static void AddHearingBooksMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(x =>
		{
			// elided...

			x.UsingRabbitMq((context,cfg) =>
			{
				cfg.Host("localhost", "/", h => {
					h.Username("guest");
					h.Password("guest");
				});

				cfg.ConfigureEndpoints(context);
			});
		});
	}
}
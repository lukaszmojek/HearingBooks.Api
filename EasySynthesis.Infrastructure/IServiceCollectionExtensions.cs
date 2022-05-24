using EasySynthesis.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EasySynthesis.Infrastructure;

public static class IServiceCollectionExtensions
{
	public static void RegisterRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IVoiceRepository, VoiceRepository>();
		services.AddScoped<ILanguageRepository, LanguageRepository>();
		services.AddScoped<ITextSynthesisRepository, TextSynthesisRepository>();
		services.AddScoped<IDialogueSynthesisRepository, DialogueSynthesisRepository>();
		services.AddScoped<IDashboardRepository, DashboardRepository>();
		services.AddScoped<ISynthesisPricingRepository, SynthesisPricingRepository>();
	}
}
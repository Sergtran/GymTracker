using GymTracker.Application.Abstractions;
using GymTracker.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymTracker.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
		services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
		return services;
	}
}
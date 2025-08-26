using Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Persistance.DataAccess;
using Redis;
using Minios;
using Utilities.Auth;
using Utilities.Services;

namespace OnlineStore.Persistance.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(
			this IServiceCollection services,
			IConfiguration configuration)
        {
                services.AddPostgres<IApplicationDbContext, ApplicationDbContext>(configuration);
                services.AddRedis(configuration);
                services.AddMinio(configuration);

		services.AddScoped<ICookieService, CookieService>();
		services.AddScoped<IPasswordHash, PasswordHash>();
		services.AddScoped<IJwt, Jwt>();

		services.AddScoped<IResetPassword, ResetPassword>();
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<IEmailQueueService, EmailQueueService>();
                services.AddHostedService<EmailQueueBackgroundService>();

		return services;
	}
}
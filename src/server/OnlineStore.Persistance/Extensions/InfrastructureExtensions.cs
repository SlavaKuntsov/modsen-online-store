using Databases;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Persistance.DataAccess;
using Utilities.Auth;
using Utilities.Service;
using Utilities.Services;

namespace OnlineStore.Persistance.Extensions;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddPostgres<IApplicationDbContext, ApplicationDbContext>(configuration);

		services.AddScoped<ICookieService, CookieService>();
		services.AddScoped<IPasswordHash, PasswordHash>();
		services.AddScoped<IJwt, Jwt>();

		services.AddScoped<IResetPassword, ResetPassword>();
		services.AddScoped<IEmailService, EmailService>();

		return services;
	}

	public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var services = scope.ServiceProvider;
		var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

		try
		{
			var context = services.GetRequiredService<ApplicationDbContext>();
			context.Database.Migrate();
			logger.LogInformation("Database migrations applied successfully");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while applying database migrations");

			throw;
		}

		return app;
	}
}
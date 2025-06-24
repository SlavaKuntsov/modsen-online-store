using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlineStore.Infrastructure.DataAccess;

namespace OnlineStore.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
								?? configuration.GetConnectionString(nameof(ApplicationDbContext));

		services.AddDbContextPool<ApplicationDbContext>(
			options =>
			{
				options.UseNpgsql(connectionString);
			},
			128);
		
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
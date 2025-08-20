using Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Persistance.DataAccess;
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

        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IPasswordHash, PasswordHash>();
        services.AddScoped<IJwt, Jwt>();

        services.AddScoped<IResetPassword, ResetPassword>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
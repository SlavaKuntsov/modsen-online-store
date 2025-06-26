using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Auth;
using Utilities.Validators;

namespace OnlineStore.Application.Extensions;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(
			cfg =>
			{
				cfg.RegisterServicesFromAssemblyContaining<UserRegistrationCommandHandler>();
			});

		// services.AddValidatorsFromAssemblyContaining<BaseCommandValidator<UserRegistrationCommand>>();
		// services
			// .AddValidatorsFromAssemblyContaining<BaseCommandValidator<UserRegistrationCommandValidator>>();

		return services;
	}
}
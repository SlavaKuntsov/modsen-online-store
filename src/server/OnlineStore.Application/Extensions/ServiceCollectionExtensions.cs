using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Auth;
using Utilities.Validators;

namespace OnlineStore.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(
			cfg =>
			{
				cfg.RegisterServicesFromAssemblyContaining<UserRegistrationCommandHandler>();
			});

		services.AddValidatorsFromAssemblyContaining<RegistrationCommandValidator>();
		// services.AddValidatorsFromAssemblyContaining<BaseCommandValidator<UserRegistrationCommand>>();
		// services
			// .AddValidatorsFromAssemblyContaining<BaseCommandValidator<UserRegistrationCommandValidator>>();

		return services;
	}
}
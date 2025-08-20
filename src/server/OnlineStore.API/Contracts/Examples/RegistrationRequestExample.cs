using OnlineStore.Application.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineStore.API.Contracts.Examples;

public class RegistrationRequestExample : IExamplesProvider<UserRegistrationCommand>
{
	public UserRegistrationCommand GetExamples()
	{
		return new UserRegistrationCommand(
			"example@gmail.com",
			"qweQWE123",
			"John",
			"Doe",
			"20-12-2020");
	}
}
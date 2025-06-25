using OnlineStore.Application.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineStore.API.Contracts.Examples;

public class LoginRequestExample : IExamplesProvider<LoginQuery>
{
	public LoginQuery GetExamples()
	{
		return new LoginQuery(
			"example@gmail.com",
			"qweQWE123");
	}
}
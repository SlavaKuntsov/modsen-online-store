using Swashbuckle.AspNetCore.Filters;

namespace OnlineStore.API.Contracts.Auth.Examples;

public class ResetPasswordExample : IExamplesProvider<ResetPasswordRequest>
{
	public ResetPasswordRequest GetExamples()
	{
		return new ResetPasswordRequest(
			"qweqweqwe",
			"qweQWE1233!");
	}
}
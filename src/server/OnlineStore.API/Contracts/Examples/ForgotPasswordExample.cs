using OnlineStore.Application.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace OnlineStore.API.Contracts.Examples;

public class ForgotPasswordExample : IExamplesProvider<ForgotPasswordCommand>
{
    public ForgotPasswordCommand GetExamples()
    {
        return new ForgotPasswordCommand("example@gmail.com");
    }
}
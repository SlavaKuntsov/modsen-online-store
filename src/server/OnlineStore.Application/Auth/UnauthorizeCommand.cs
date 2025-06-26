using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Auth;

public sealed record UnauthorizeCommand(Guid Id) : IRequest;

public sealed class UnauthorizeCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<UnauthorizeCommand>
{
	public async Task Handle(UnauthorizeCommand request, CancellationToken ct = default)
	{
		await dbContext.RefreshTokens
			.Where(t => t.Id == request.Id)
			.ExecuteUpdateAsync(
				s => s
					.SetProperty(t => t.IsRevoked, true),
				ct);
	}
}
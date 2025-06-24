using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;
using Utilities.Auth;

namespace OnlineStore.Application.Tokens;

public sealed record GenerateTokensCommand(Guid UserId, Role Role) : IRequest<AuthDto>;

public class GenerateTokensCommandHandler(
	IApplicationDbContext dbContext,
	IJwt jwt)
	: IRequestHandler<GenerateTokensCommand, AuthDto>
{
	public async Task<AuthDto> Handle(GenerateTokensCommand request, CancellationToken cancellationToken)
	{
		var accessToken = jwt.GenerateAccessToken(request.UserId, request.Role);
		var newRefreshToken = jwt.GenerateRefreshToken();

		var newRefreshTokenModel = new RefreshToken(
			request.UserId,
			newRefreshToken,
			jwt.GetRefreshTokenExpirationDays());

		var existRefreshToken = await dbContext.RefreshTokens
			.AsNoTracking()
			.Where(t => t.UserId == request.UserId)
			.FirstOrDefaultAsync(cancellationToken);

		if (existRefreshToken is not null)
		{
			// existRefreshToken.Token = newRefreshTokenModel.Token;
			// existRefreshToken.ExpiresAt = newRefreshTokenModel.ExpiresAt;
			// dbContext.RefreshTokens.Attach(existRefreshToken).State = EntityState.Modified;
		
			await dbContext.RefreshTokens
				.Where(t => t.Id == existRefreshToken.Id)
				.ExecuteUpdateAsync(
					s => s
						.SetProperty(t => t.Token, newRefreshTokenModel.Token)
						.SetProperty(t => t.ExpiresAt, newRefreshTokenModel.ExpiresAt),
					cancellationToken);
		}
		else
		{
			await dbContext.RefreshTokens.AddAsync(newRefreshTokenModel, cancellationToken);
		}
		
		await dbContext.SaveChangesAsync(cancellationToken);
		
		return new AuthDto(accessToken, newRefreshToken);
	}
}
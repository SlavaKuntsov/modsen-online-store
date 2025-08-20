using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using Utilities.Auth;

namespace OnlineStore.Application.Auth;

public record struct ResetPasswordCommand(Guid? UserId, string Token, string NewPassword) : IRequest;

public sealed class ResetPasswordCommandHandler(
	IApplicationDbContext dbContext,
	IPasswordHash passwordHash,
	IResetPassword resetPassword)
	: IRequestHandler<ResetPasswordCommand>
{

	public async Task Handle(ResetPasswordCommand request, CancellationToken ct = default)
	{
		var existResetToken = await dbContext.PasswordResetTokens
			.FirstOrDefaultAsync(x => x.UserId == request.UserId
									&& x.Token == request.Token
									&& !x.IsUsed,
				ct);

		if (existResetToken is null)
			throw new NotFoundException("Reset token not found");

		if (!resetPassword.ValidateTokenAsync(existResetToken))
			throw new UnauthorizedAccessException("Invalid or expired token");

		var user = await dbContext.Users
			.Where(u => u.Id == request.UserId)
			.FirstOrDefaultAsync(ct);

		if (user is null)
			throw new KeyNotFoundException("User not found");

		user.PasswordHash = passwordHash.Generate(request.NewPassword);

		existResetToken = await dbContext.PasswordResetTokens
			.FirstOrDefaultAsync(x => x.UserId == request.UserId
									&& x.Token == request.Token,
				ct);

		if (existResetToken is not null)
		{
			existResetToken.IsUsed = true;
		}

		await dbContext.SaveChangesAsync(ct);
	}
}
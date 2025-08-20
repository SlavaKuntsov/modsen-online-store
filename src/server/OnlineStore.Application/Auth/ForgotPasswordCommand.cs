using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Domain.Entities;
using Utilities.Auth;
using Utilities.Services;

namespace OnlineStore.Application.Auth;

public sealed record ForgotPasswordCommand(string Email) : IRequest;

public sealed class ForgotPasswordCommandHandler(
    IApplicationDbContext dbContext,
    IResetPassword resetPassword,
    IEmailService emailService)
    : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken ct = default)
    {
        var user = await dbContext.Users
            .Where(u => u.Email == request.Email)
            .FirstOrDefaultAsync(ct);

        if (user is null)
            return;

        // invalidate all old tokens
        var activeTokens = await dbContext.PasswordResetTokens
            .Where(x => x.UserId == user.Id && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);

        foreach (var token in activeTokens)
            token.IsUsed = true;

        var tokenValue = Uri.EscapeDataString(resetPassword.GenerateResetToken());

        var tokenEntity = new PasswordResetToken(
            user.Id,
            tokenValue,
            resetPassword.GetResetTokenExpirationMinutes());

        await dbContext.PasswordResetTokens.AddAsync(tokenEntity, ct);

        await dbContext.SaveChangesAsync(ct);

        // var resetLink = $"https://frontend.example.com/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        var resetLink = tokenValue;

        await emailService.SendEmailAsync(
            user.Email,
            "Password Reset",
            $"""
			Reset token: 
			<div style="font-family: monospace; background-color: #f5f5f5; padding: 10px; border-radius: 5px; word-break: break-all;">
				{resetLink}
			</div>
			"""
        );
    }
}
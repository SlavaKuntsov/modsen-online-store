using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using Utilities.Auth;

namespace OnlineStore.Application.Tokens;

public sealed record GetByRefreshTokenQuery(string RefreshToken) : IRequest<UserRoleDto>;

public sealed class GetByRefreshTokenQueryHandler(
    IApplicationDbContext dbContext,
    IJwt jwt)
    : IRequestHandler<GetByRefreshTokenQuery, UserRoleDto>
{
    public async Task<UserRoleDto> Handle(GetByRefreshTokenQuery request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existRefreshToken = await dbContext.RefreshTokens
            .AsNoTracking()
            .Where(t => t.Token == request.RefreshToken)
            .FirstOrDefaultAsync(ct);

        var userId = jwt.ValidateRefreshTokenAsync(existRefreshToken);

        if (userId == Guid.Empty)
            throw new InvalidTokenException("Invalid refresh token");

        var userEntity = await dbContext.Users
            .AsNoTracking()
            .Where(t => t.Id == userId)
            .FirstOrDefaultAsync(ct);

        if (userEntity is null)
            throw new NotFoundException("User not found");

        return new UserRoleDto(userId, userEntity.Role);
    }
}
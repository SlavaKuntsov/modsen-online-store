using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using Utilities.Auth;

namespace OnlineStore.Application.Auth;

public sealed record LoginQuery(string Email, string Password) : IRequest<UserRoleDto>;

public sealed class LoginQueryHandler(
	IApplicationDbContext dbContext,
	IPasswordHash passwordHash) 
	: IRequestHandler<LoginQuery, UserRoleDto>
{
	public async Task<UserRoleDto> Handle(LoginQuery request, CancellationToken ct = default)
	{
		var existUser = await dbContext.Users
			.AsNoTracking()
			.Where(u => u.Email == request.Email)
			.FirstOrDefaultAsync(ct);

		if (existUser is null)
			throw new NotFoundException($"User with email '{request.Email}' not found.");

		var isCorrectPassword = passwordHash.Verify(request.Password, existUser.PasswordHash);

		if (!isCorrectPassword)
			throw new UnauthorizedAccessException("Incorrect password");

		return new UserRoleDto(existUser.Id, existUser.Role);
	}
}
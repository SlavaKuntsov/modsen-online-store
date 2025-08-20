using Domain.Enums;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;
using Utilities.Auth;

namespace OnlineStore.Application.Auth;

public sealed record UserRegistrationCommand(
	string Email,
	string Password,
	string FirstName,
	string LastName,
	string DateOfBirth) : IRequest<AuthDto>;

public sealed class UserRegistrationCommandHandler(
	IJwt jwt,
	IPasswordHash passwordHash,
	IValidator<UserRegistrationCommand> validator,
	IApplicationDbContext dbContext) : IRequestHandler<UserRegistrationCommand, AuthDto>
{
	public async Task<AuthDto> Handle(
		UserRegistrationCommand request,
		CancellationToken ct = default)
	{
		var validationResult = await validator.ValidateAsync(request, ct);
		if (!validationResult.IsValid)
			throw new ValidationException(validationResult.Errors);

		var existUser = await dbContext.Users
			.AsNoTracking()
			.Where(u => u.Email == request.Email)
			.FirstOrDefaultAsync(ct);

		if (existUser is not null)
			throw new AlreadyExistsException($"User with email {request.Email} already exists");

		const Role role = Role.User;

		var userEntity = new User(
			request.Email,
			passwordHash.Generate(request.Password),
			role,
			request.FirstName,
			request.LastName,
			request.DateOfBirth
		);

		var accessToken = jwt.GenerateAccessToken(userEntity.Id, role);
		var refreshToken = jwt.GenerateRefreshToken();

		var refreshTokenEntity = new RefreshToken(
			userEntity.Id,
			refreshToken,
			jwt.GetRefreshTokenExpirationDays());

		await dbContext.Users.AddAsync(userEntity, ct);
		await dbContext.RefreshTokens.AddAsync(refreshTokenEntity, ct);

		await dbContext.SaveChangesAsync(ct);

		return new AuthDto(accessToken, refreshToken);
	}
}
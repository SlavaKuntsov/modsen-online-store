using Common.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Users;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

public sealed class GetUserByIdQueryHandler(IApplicationDbContext dbContext)
	: IRequestHandler<GetUserByIdQuery, UserDto?>
{
	public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var userEntity = await dbContext.Users
			.AsNoTracking()
			.Where(u => u.Id == request.Id)
			.FirstOrDefaultAsync(cancellationToken);

		if (userEntity is null)
			throw new NotFoundException($"User with id '{request.Id}' not found");

		var dto = new UserDto(
			userEntity.Id,
			userEntity.Email,
			userEntity.Role.GetDescription(),
			userEntity.FirstName,
			userEntity.LastName,
			userEntity.DateOfBirth);

		return dto;
	}
}
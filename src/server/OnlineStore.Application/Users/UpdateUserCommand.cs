using Domain.Exceptions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Users;

public record struct UpdateUserCommand(
    Guid? Id,
    string FirstName,
    string LastName,
    string DateOfBirth) : IRequest<UserDto>;

public class UpdateUserCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper) : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct = default)
    {
        var userId = request.Id
                    ?? throw new ArgumentNullException(nameof(request.Id), "User ID is required.");

        var existUser = await dbContext.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(ct);

        if (existUser is null)
            throw new NotFoundException($"User with id {request.Id} doesn't exists");

        request.Adapt(existUser);

        await dbContext.SaveChangesAsync(ct);

        return mapper.Map<UserDto>(existUser);
    }
}
using Domain.Enums;

namespace OnlineStore.Application.Dtos;

public sealed record UserRoleDto(
    Guid Id,
    Role Role);
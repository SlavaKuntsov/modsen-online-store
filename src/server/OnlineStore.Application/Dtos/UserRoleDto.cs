using Domain.Enums;

namespace OnlineStore.Application.Dtos;

public record UserRoleDto(
	Guid Id,
	Role Role);
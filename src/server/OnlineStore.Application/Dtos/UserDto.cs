namespace OnlineStore.Application.Dtos;

public record UserDto(
	Guid Id,
	string Email,
	string Role,
	string FirstName,
	string LastName,
	string DateOfBirth);
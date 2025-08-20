namespace OnlineStore.Application.Dtos;

public sealed record UserDto(
    Guid Id,
    string Email,
    string Role,
    string FirstName,
    string LastName,
    string DateOfBirth);
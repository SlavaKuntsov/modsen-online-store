namespace OnlineStore.Application.Dtos;

public record AuthDto(
	string AccessToken,
	string RefreshToken);
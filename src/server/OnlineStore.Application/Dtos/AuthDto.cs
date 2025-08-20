namespace OnlineStore.Application.Dtos;

public sealed record AuthDto(
	string AccessToken,
	string RefreshToken);
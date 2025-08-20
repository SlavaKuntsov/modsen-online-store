namespace OnlineStore.Application.Dtos;

public sealed record TokensDto(
	string AccessToken,
	string RefreshToken);
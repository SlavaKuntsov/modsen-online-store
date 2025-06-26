namespace OnlineStore.Application.Dtos;

public record TokensDto(
	string AccessToken,
	string RefreshToken);
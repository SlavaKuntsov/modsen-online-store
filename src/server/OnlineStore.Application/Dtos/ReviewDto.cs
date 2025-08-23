namespace OnlineStore.Application.Dtos;

public record ReviewDto(
		Guid Id,
		Guid ProductId,
		Guid UserId,
		int Rating,
		string Comment,
		DateTime CreatedAt);
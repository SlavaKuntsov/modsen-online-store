namespace OnlineStore.Application.Dtos;

public record ProductDto(
		Guid Id,
		string Name,
		string Description,
		decimal Price,
		int StockQuantity,
		Guid CategoryId,
		double Rating,
		int Popularity,
				DateTime CreatedAt,
				string? ImageUrl);
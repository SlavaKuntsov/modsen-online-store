namespace OnlineStore.Application.Dtos;

public record OrderItemDto(
		Guid ProductId,
		string ProductName,
		decimal UnitPrice,
		int Quantity,
		decimal SubTotal);
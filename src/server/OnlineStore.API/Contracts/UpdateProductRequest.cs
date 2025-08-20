namespace OnlineStore.API.Contracts;

public record UpdateProductRequest(
	string Name,
	string Description,
	decimal Price,
	int StockQuantity,
	Guid CategoryId);
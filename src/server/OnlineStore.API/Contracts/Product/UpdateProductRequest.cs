namespace OnlineStore.API.Contracts.Product;

public record UpdateProductRequest(
	string Name,
	string Description,
	decimal Price,
	int StockQuantity,
	Guid CategoryId);
namespace OnlineStore.API.Contracts.Product;

public record CreateProductRequest(
	string Name,
	string Description,
	decimal Price,
	int StockQuantity,
	Guid CategoryId);
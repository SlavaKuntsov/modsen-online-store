namespace OnlineStore.API.Contracts;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId);

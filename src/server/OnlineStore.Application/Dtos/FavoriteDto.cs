namespace OnlineStore.Application.Dtos;

public record FavoriteDto(
        Guid ProductId,
        string ProductName,
        decimal Price);

namespace OnlineStore.Application.Dtos;

public record PromoCodeDto(
        Guid Id,
        string Code,
        decimal DiscountPercentage,
        DateTime ExpirationDate,
        bool IsActive);

namespace OnlineStore.API.Contracts.PromoCode;

public record CreatePromoCodeRequest(
		string Code,
		decimal DiscountPercentage,
		DateTime ExpirationDate,
		bool IsActive);
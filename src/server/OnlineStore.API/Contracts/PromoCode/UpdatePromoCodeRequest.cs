namespace OnlineStore.API.Contracts.PromoCode;

public record UpdatePromoCodeRequest(
		string Code,
		decimal DiscountPercentage,
		DateTime ExpirationDate,
		bool IsActive);
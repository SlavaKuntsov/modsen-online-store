namespace OnlineStore.Domain.Entities;

public class PromoCode
{
	public Guid Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public decimal DiscountPercentage { get; set; }
	public DateTime ExpirationDate { get; set; }
	public bool IsActive { get; set; }

	public PromoCode() { }

	public PromoCode(string code, decimal discountPercentage, DateTime expirationDate, bool isActive = true)
	{
		Id = Guid.NewGuid();
		Code = code;
		DiscountPercentage = discountPercentage;
		ExpirationDate = expirationDate;
		IsActive = isActive;
	}
}
using MediatR;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.PromoCodes;

public sealed record CreatePromoCodeCommand(
		string Code,
		decimal DiscountPercentage,
		DateTime ExpirationDate,
		bool IsActive = true) : IRequest<PromoCodeDto>;

public sealed class CreatePromoCodeCommandHandler(IApplicationDbContext dbContext)
		: IRequestHandler<CreatePromoCodeCommand, PromoCodeDto>
{
	public async Task<PromoCodeDto> Handle(CreatePromoCodeCommand request, CancellationToken ct)
	{
		PromoCode promo = new(request.Code, request.DiscountPercentage, request.ExpirationDate, request.IsActive);
		await dbContext.PromoCodes.AddAsync(promo, ct);
		await dbContext.SaveChangesAsync(ct);
		return new(promo.Id, promo.Code, promo.DiscountPercentage, promo.ExpirationDate, promo.IsActive);
	}
}
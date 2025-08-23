using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.PromoCodes;

public sealed record UpdatePromoCodeCommand(
		Guid Id,
		string Code,
		decimal DiscountPercentage,
		DateTime ExpirationDate,
		bool IsActive) : IRequest<PromoCodeDto>;

public sealed class UpdatePromoCodeCommandHandler(IApplicationDbContext dbContext)
		: IRequestHandler<UpdatePromoCodeCommand, PromoCodeDto>
{
	public async Task<PromoCodeDto> Handle(UpdatePromoCodeCommand request, CancellationToken ct)
	{
		var promo = await dbContext.PromoCodes.FirstOrDefaultAsync(p => p.Id == request.Id, ct)
				?? throw new InvalidOperationException("Promo code not found");

		promo.Code = request.Code;
		promo.DiscountPercentage = request.DiscountPercentage;
		promo.ExpirationDate = request.ExpirationDate;
		promo.IsActive = request.IsActive;
		await dbContext.SaveChangesAsync(ct);

		return new(promo.Id, promo.Code, promo.DiscountPercentage, promo.ExpirationDate, promo.IsActive);
	}
}
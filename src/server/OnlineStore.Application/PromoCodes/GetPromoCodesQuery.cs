using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.PromoCodes;

public sealed record GetPromoCodesQuery() : IRequest<List<PromoCodeDto>>;

public sealed class GetPromoCodesQueryHandler(IApplicationDbContext dbContext)
		: IRequestHandler<GetPromoCodesQuery, List<PromoCodeDto>>
{
	public async Task<List<PromoCodeDto>> Handle(GetPromoCodesQuery request, CancellationToken ct)
	{
		var promos = await dbContext.PromoCodes.ToListAsync(ct);
		return promos.Select(p => new PromoCodeDto(p.Id, p.Code, p.DiscountPercentage, p.ExpirationDate, p.IsActive)).ToList();
	}
}
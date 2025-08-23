using MediatR;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.PromoCodes;

public sealed record DeletePromoCodeCommand(Guid Id) : IRequest;

public sealed class DeletePromoCodeCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<DeletePromoCodeCommand>
{
	public async Task Handle(DeletePromoCodeCommand request, CancellationToken ct)
	{
		var promo = await dbContext.PromoCodes.FindAsync([request.Id], ct)
			?? throw new InvalidOperationException("Promo code not found");

		dbContext.PromoCodes.Remove(promo);
		await dbContext.SaveChangesAsync(ct);
	}
}
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Categories;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest;

public sealed class DeleteCategoryCommandHandler(IApplicationDbContext dbContext)
		: IRequestHandler<DeleteCategoryCommand>
{
	public async Task Handle(DeleteCategoryCommand request, CancellationToken ct)
	{
		var category = await dbContext.Categories
				.Where(c => c.Id == request.Id)
				.FirstOrDefaultAsync(ct);

		if (category is null)
			throw new NotFoundException($"Category with id '{request.Id}' not found");

		dbContext.Categories.Remove(category);
		await dbContext.SaveChangesAsync(ct);
	}
}
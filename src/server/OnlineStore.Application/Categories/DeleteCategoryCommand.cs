using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;

namespace OnlineStore.Application.Categories;

public sealed record DeleteCategoryCommand(Guid Id, bool DeleteAll = false) : IRequest;

public sealed class DeleteCategoryCommandHandler(IApplicationDbContext dbContext)
		: IRequestHandler<DeleteCategoryCommand>
{
	public async Task Handle(DeleteCategoryCommand request, CancellationToken ct)
	{
		var category = await dbContext.Categories
			.Include(c => c.SubCategories)
			.Where(c => c.Id == request.Id)
			.FirstOrDefaultAsync(ct);

		if (category is null)
			throw new NotFoundException($"Category with id '{request.Id}' not found");

		if (category.SubCategories.Any() && !request.DeleteAll)
			throw new BadRequestException("Category has subcategories. Set deleteAll=true to delete them.");

		dbContext.Categories.Remove(category);
		await dbContext.SaveChangesAsync(ct);
	}
}
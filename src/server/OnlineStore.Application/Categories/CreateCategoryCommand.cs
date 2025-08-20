using MediatR;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Categories;

public sealed record CreateCategoryCommand(string Name, Guid? ParentCategoryId = null) : IRequest<CategoryDto>;

public sealed class CreateCategoryCommandHandler(IApplicationDbContext dbContext)
		: IRequestHandler<CreateCategoryCommand, CategoryDto>
{
	public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken ct)
	{
		var category = new Category(request.Name, request.ParentCategoryId);

		await dbContext.Categories.AddAsync(category, ct);
		await dbContext.SaveChangesAsync(ct);

		return new CategoryDto(category.Id, category.Name, category.ParentCategoryId, new List<CategoryDto>());
	}
}
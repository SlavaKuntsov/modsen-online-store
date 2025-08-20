using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Abstractions.Data;
using OnlineStore.Application.Dtos;

namespace OnlineStore.Application.Categories;

public sealed record GetCategoryTreeQuery() : IRequest<List<CategoryDto>>;

public sealed class GetCategoryTreeQueryHandler(IApplicationDbContext dbContext)
		: IRequestHandler<GetCategoryTreeQuery, List<CategoryDto>>
{
	public async Task<List<CategoryDto>> Handle(GetCategoryTreeQuery request, CancellationToken ct)
	{
		var categories = await dbContext.Categories.AsNoTracking().ToListAsync(ct);

		var dict = categories.ToDictionary(
				c => c.Id,
				c => new CategoryDto(c.Id, c.Name, c.ParentCategoryId, new List<CategoryDto>()));

		var roots = new List<CategoryDto>();

		foreach (var dto in dict.Values)
		{
			if (dto.ParentCategoryId.HasValue && dict.TryGetValue(dto.ParentCategoryId.Value, out var parent))
			{
				parent.SubCategories.Add(dto);
			}
			else
			{
				roots.Add(dto);
			}
		}

		return roots;
	}
}
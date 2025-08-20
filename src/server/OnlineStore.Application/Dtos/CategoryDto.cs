namespace OnlineStore.Application.Dtos;

public record CategoryDto(Guid Id, string Name, Guid? ParentCategoryId, List<CategoryDto> SubCategories);
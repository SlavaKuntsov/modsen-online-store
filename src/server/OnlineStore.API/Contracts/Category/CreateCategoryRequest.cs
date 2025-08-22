namespace OnlineStore.API.Contracts.Category;

public sealed record CreateCategoryRequest(string Name, string? ParentCategoryId);
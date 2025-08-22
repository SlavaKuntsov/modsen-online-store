namespace OnlineStore.API.Contracts.Category;

public sealed record UpdateCategoryRequest(string Name, string? ParentCategoryId);
namespace OnlineStore.API.Contracts;

public sealed record CreateCategoryRequest(string Name, string? ParentCategoryId);

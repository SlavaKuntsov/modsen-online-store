namespace OnlineStore.API.Contracts;

public sealed record UpdateCategoryRequest(string Name, string? ParentCategoryId);
namespace OnlineStore.API.Contracts;

public record ApiResponse<T>(
    int StatusCode,
    T? Data,
    int? Total = null,
    string? Message = null
);

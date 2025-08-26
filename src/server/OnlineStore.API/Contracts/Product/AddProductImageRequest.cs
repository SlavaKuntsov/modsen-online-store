namespace OnlineStore.API.Contracts.Product;

using Microsoft.AspNetCore.Http;

public record AddProductImageRequest(IFormFile File);


namespace OnlineStore.API.Contracts.Product;

using Microsoft.AspNetCore.Http;

public record UpdateProductImageRequest(IFormFile File);
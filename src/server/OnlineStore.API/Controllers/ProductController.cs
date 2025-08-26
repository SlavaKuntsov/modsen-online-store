using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineStore.API.Contracts;
using OnlineStore.API.Contracts.Product;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Products;
using OnlineStore.Application.ProductImages;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/products")]
[ApiVersion("1.0")]
public class ProductController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get(
		[FromQuery] Guid? categoryId,
		[FromQuery] decimal? minPrice,
		[FromQuery] decimal? maxPrice,
		[FromQuery] double? minRating,
		[FromQuery] bool? inStock,
		[FromQuery] string? sortBy,
		[FromQuery] bool descending = false,
		CancellationToken ct = default)
	{
		var products = await mediator.Send(new GetProductsQuery(
				categoryId,
				minPrice,
				maxPrice,
				minRating,
				inStock,
				sortBy,
				descending), ct);
		return Ok(new ApiResponse<List<ProductDto>>(StatusCodes.Status200OK, products, products.Count));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
	{
		var product = await mediator.Send(new GetProductByIdQuery(id), ct);
		return Ok(new ApiResponse<ProductDto>(StatusCodes.Status200OK, product, 1));
	}

	[HttpPost]
	[Authorize(Policy = "Admin")]
	public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct = default)
	{
		var product = await mediator.Send(new CreateProductCommand(
				request.Name,
				request.Description,
				request.Price,
				request.StockQuantity,
				request.CategoryId), ct);
		return StatusCode(StatusCodes.Status201Created,
				new ApiResponse<ProductDto>(StatusCodes.Status201Created, product, 1));
	}

	[HttpPut("{id}")]
	[Authorize(Policy = "Admin")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct = default)
	{
		var product = await mediator.Send(new UpdateProductCommand(
				id,
				request.Name,
				request.Description,
				request.Price,
				request.StockQuantity,
				request.CategoryId), ct);
		return Ok(new ApiResponse<ProductDto>(StatusCodes.Status200OK, product, 1));
	}

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
                await mediator.Send(new DeleteProductCommand(id), ct);
                return Ok(new ApiResponse<string>(StatusCodes.Status200OK, "Deleted", 0));
        }

        [HttpPost("{id}/images")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddImage(Guid id, [FromForm] IFormFile file, CancellationToken ct = default)
        {
                var url = await mediator.Send(new AddProductImageCommand(id, file), ct);
                return Ok(new ApiResponse<string>(StatusCodes.Status200OK, url, 1));
        }

        [HttpPut("{id}/images/{imageId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateImage(Guid id, Guid imageId, [FromForm] IFormFile file, CancellationToken ct = default)
        {
                var url = await mediator.Send(new UpdateProductImageCommand(imageId, file), ct);
                return Ok(new ApiResponse<string>(StatusCodes.Status200OK, url, 1));
        }

        [HttpDelete("{id}/images/{imageId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId, CancellationToken ct = default)
        {
                await mediator.Send(new DeleteProductImageCommand(imageId), ct);
                return Ok(new ApiResponse<string>(StatusCodes.Status200OK, "Deleted", 0));
        }
}
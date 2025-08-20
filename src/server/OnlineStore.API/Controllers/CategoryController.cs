using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.Application.Categories;
using OnlineStore.Application.Dtos;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/categories")]
[ApiVersion("1.0")]
public class CategoryController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get(CancellationToken ct = default)
	{
                var categories = await mediator.Send(new GetCategoryTreeQuery(), ct);
                return Ok(new ApiResponse<List<CategoryDto>>(StatusCodes.Status200OK, categories, categories.Count));
        }

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct = default)
	{
		Guid? parentId = string.IsNullOrWhiteSpace(request.ParentCategoryId)
			? null
			: Guid.Parse(request.ParentCategoryId);

                var category = await mediator.Send(new CreateCategoryCommand(request.Name, parentId), ct);
                return StatusCode(StatusCodes.Status201Created,
                        new ApiResponse<CategoryDto>(StatusCodes.Status201Created, category, 1));
        }

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct = default)
	{
		Guid? parentId = string.IsNullOrWhiteSpace(request.ParentCategoryId)
			? null
			: Guid.Parse(request.ParentCategoryId);

                var category = await mediator.Send(new UpdateCategoryCommand(id, request.Name, parentId), ct);
                return Ok(new ApiResponse<CategoryDto>(StatusCodes.Status200OK, category, 1));
        }

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id, [FromQuery] bool deleteAll = false, CancellationToken ct = default)
	{
                await mediator.Send(new DeleteCategoryCommand(id, deleteAll), ct);
                return Ok(new ApiResponse<string>(StatusCodes.Status200OK, "Deleted", 0));
        }
}
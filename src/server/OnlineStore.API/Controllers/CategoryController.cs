using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.Application.Categories;

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
		return Ok(categories);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct = default)
	{
		Guid? parentId = string.IsNullOrWhiteSpace(request.ParentCategoryId)
			? null
			: Guid.Parse(request.ParentCategoryId);

		var category = await mediator.Send(new CreateCategoryCommand(request.Name, parentId), ct);
		return Ok(category);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct = default)
	{
		Guid? parentId = string.IsNullOrWhiteSpace(request.ParentCategoryId)
			? null
			: Guid.Parse(request.ParentCategoryId);

		var category = await mediator.Send(new UpdateCategoryCommand(id, request.Name, parentId), ct);
		return Ok(category);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id, [FromQuery] bool deleteAll = false, CancellationToken ct = default)
	{
		await mediator.Send(new DeleteCategoryCommand(id, deleteAll), ct);
		return NoContent();
	}
}
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
	public async Task<IActionResult> Create([FromBody] CreateCategoryCommand request, CancellationToken ct = default)
	{
		var category = await mediator.Send(request, ct);
		return Ok(category);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand request, CancellationToken ct = default)
	{
		var command = request with { Id = id };
		var category = await mediator.Send(command, ct);
		return Ok(category);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
	{
		await mediator.Send(new DeleteCategoryCommand(id), ct);
		return NoContent();
	}
}
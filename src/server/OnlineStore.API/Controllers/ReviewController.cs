using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.API.Contracts.Review;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Reviews;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/reviews")]
[ApiVersion("1.0")]
public class ReviewController(IMediator mediator) : ControllerBase
{
        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> Get(Guid productId, CancellationToken ct = default)
        {
                var reviews = await mediator.Send(new GetReviewsQuery(productId), ct);
                return Ok(new ApiResponse<List<ReviewDto>>(StatusCodes.Status200OK, reviews, reviews.Count));
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest request, CancellationToken ct = default)
        {
                var review = await mediator.Send(new CreateReviewCommand(request.ProductId, request.Rating, request.Comment), ct);
                return StatusCode(StatusCodes.Status201Created,
                                new ApiResponse<ReviewDto>(StatusCodes.Status201Created, review, 1));
        }
}

using System.Security.Claims;
using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Users;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UserController(IMediator mediator, IMapper mapper)
    : ControllerBase
{
    [HttpGet("update")]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] UpdateUserCommand request,
        CancellationToken ct = default)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                        ?? throw new UnauthorizedAccessException("User ID not found in claims.");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid User ID format in claims.");

        var command = request with { Id = userId };

        var user = await mediator.Send(command, ct);

        return Ok(mapper.Map<UserDto>(user));
    }
}
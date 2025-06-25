using System.Security.Claims;
using Asp.Versioning;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts.Examples;
using OnlineStore.Application.Auth;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Tokens;
using OnlineStore.Application.Users;
using Swashbuckle.AspNetCore.Filters;
using Utilities.Service;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthControllers(
	IMediator mediator,
	ICookieService cookieService)
	: ControllerBase
{
	[HttpGet("refresh-token")]
	public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
	{
		var refreshToken = cookieService.GetRefreshToken();

		var userRoleDto = await mediator.Send(
			new GetByRefreshTokenQuery(
				refreshToken),
			cancellationToken);

		var authResultDto = await mediator.Send(
			new GenerateTokensCommand(userRoleDto.Id, userRoleDto.Role),
			cancellationToken);

		HttpContext.Response.Cookies.Append(
			JwtConstants.RefreshCookieName,
			authResultDto.RefreshToken);

		return Ok(new { authResultDto.AccessToken, authResultDto.RefreshToken });
	}

	[HttpGet("authorize")]
	[Authorize(Policy = "All")]
	public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) 
						?? throw new UnauthorizedAccessException("User ID not found in claims.");

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		var user = await mediator.Send(
			new GetUserByIdQuery(userId),
			cancellationToken);

		return Ok(user);
	}

	[HttpGet("unauthorize")]
	[Authorize(Policy = "All")]
	public async Task<IActionResult> Unauthorize(CancellationToken cancellationToken)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
						?? throw new UnauthorizedAccessException("User ID not found in claims.");

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		cookieService.DeleteRefreshToken();

		await mediator.Send(new UnauthorizeCommand(userId), cancellationToken);

		return Ok();
	}
	
	[HttpPost("login")]
	[SwaggerRequestExample(typeof(LoginQuery), typeof(LoginRequestExample))]
	public async Task<IActionResult> Login([FromBody] LoginQuery request, CancellationToken cancellationToken)
	{
		var existUser = await mediator.Send(
			new LoginQuery(request.Email, request.Password),
			cancellationToken);

		var authResultDto = await mediator.Send(
			new GenerateTokensCommand(existUser.Id, existUser.Role), cancellationToken);

		HttpContext.Response.Cookies.Append(
			JwtConstants.RefreshCookieName,
			authResultDto.RefreshToken);

		return Ok(new TokensDto(
			authResultDto.AccessToken,
			authResultDto.RefreshToken));
	}

	[HttpPost("registration")]
	[SwaggerRequestExample(typeof(UserRegistrationCommand), typeof(RegistrationRequestExample))]
	public async Task<IActionResult> Registration([FromBody] UserRegistrationCommand request, CancellationToken cancellationToken)
	{
		var authResultDto = await mediator.Send(request, cancellationToken);

		return Ok(new TokensDto(
			authResultDto.AccessToken,
			authResultDto.RefreshToken));
	}
}
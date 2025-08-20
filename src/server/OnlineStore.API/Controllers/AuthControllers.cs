using System.Security.Claims;
using Asp.Versioning;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Contracts;
using OnlineStore.API.Contracts.Examples;
using OnlineStore.Application.Auth;
using OnlineStore.Application.Dtos;
using OnlineStore.Application.Tokens;
using OnlineStore.Application.Users;
using Swashbuckle.AspNetCore.Filters;
using Utilities.Services;

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
	public async Task<IActionResult> RefreshToken(CancellationToken ct = default)
	{
		var refreshToken = cookieService.GetRefreshToken();

		var userRoleDto = await mediator.Send(
			new GetByRefreshTokenQuery(
				refreshToken),
			ct);

		var authResultDto = await mediator.Send(
			new GenerateTokensCommand(userRoleDto.Id, userRoleDto.Role),
			ct);

		HttpContext.Response.Cookies.Append(
			JwtConstants.RefreshCookieName,
			authResultDto.RefreshToken);

		return Ok(new { authResultDto.AccessToken, authResultDto.RefreshToken });
	}

	[HttpGet("authorize")]
	[Authorize(Policy = "All")]
	public async Task<IActionResult> Authorize(CancellationToken ct = default)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
						?? throw new UnauthorizedAccessException("User ID not found in claims.");

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		var user = await mediator.Send(
			new GetUserByIdQuery(userId),
			ct);

		return Ok(user);
	}

	[HttpGet("unauthorize")]
	[Authorize(Policy = "All")]
	public async Task<IActionResult> Unauthorize(CancellationToken ct = default)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
						?? throw new UnauthorizedAccessException("User ID not found in claims.");

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		cookieService.DeleteRefreshToken();

		await mediator.Send(new UnauthorizeCommand(userId), ct);

		return Ok();
	}

	[HttpPost("login")]
	[SwaggerRequestExample(typeof(LoginQuery), typeof(LoginRequestExample))]
	public async Task<IActionResult> Login([FromBody] LoginQuery request, CancellationToken ct = default)
	{
		var existUser = await mediator.Send(
			new LoginQuery(request.Email, request.Password),
			ct);

		var authResultDto = await mediator.Send(
			new GenerateTokensCommand(existUser.Id, existUser.Role), ct);

		HttpContext.Response.Cookies.Append(
			JwtConstants.RefreshCookieName,
			authResultDto.RefreshToken);

		return Ok(new TokensDto(
			authResultDto.AccessToken,
			authResultDto.RefreshToken));
	}

	[HttpPost("registration")]
	[SwaggerRequestExample(typeof(UserRegistrationCommand), typeof(RegistrationRequestExample))]
	public async Task<IActionResult> Registration([FromBody] UserRegistrationCommand request, CancellationToken ct = default)
	{
		var authResultDto = await mediator.Send(request, ct);

		return Ok(new TokensDto(
			authResultDto.AccessToken,
			authResultDto.RefreshToken));
	}

	[HttpPost("forgot-password")]
	[SwaggerRequestExample(typeof(ForgotPasswordCommand), typeof(ForgotPasswordExample))]
	public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request, CancellationToken ct = default)
	{
		await mediator.Send(request, ct);

		return Ok(new { message = "If the email exists, a reset link was sent." });
	}

	[HttpPost("reset-password")]
	[SwaggerRequestExample(typeof(ResetPasswordRequest), typeof(ResetPasswordExample))]
	[Authorize(Policy = "All")]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request, CancellationToken ct = default)
	{
		var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
						?? throw new UnauthorizedAccessException("User ID not found in claims.");

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			throw new UnauthorizedAccessException("Invalid User ID format in claims.");

		var command = request with { UserId = userId };

		await mediator.Send(command, ct);

		return Ok();
	}
}
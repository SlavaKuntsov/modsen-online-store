using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Common.Authorization;
using Common.Enums;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Application.Abstractions.Data;
using Utilities.Auth;

namespace OnlineStore.Infrastructure.Helpers.Auth;

public class Jwt(IOptions<JwtOptions> jwtOptions, IApplicationDbContext dbContext)
	: IJwt
{
	private readonly JwtOptions _jwtOptions = jwtOptions.Value;

	public string GenerateAccessToken(Guid id, Role role)
	{
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, id.ToString()),
			new Claim(ClaimTypes.Role, role.GetDescription())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		var randomBytes = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);

		return Convert.ToBase64String(randomBytes);
	}

	public async Task<Guid> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
	{
		var storedToken = await dbContext.RefreshTokens
			.AsNoTracking()
			.Where(t => t.Token == refreshToken)
			.FirstOrDefaultAsync(cancellationToken);

		if (storedToken?.UserId == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
			return Guid.Empty;

		return storedToken.UserId.Value;
	}

	public int GetRefreshTokenExpirationDays()
	{
		return _jwtOptions.RefreshTokenExpirationDays;
	}
}
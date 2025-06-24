using Domain.Enums;

namespace Utilities.Auth;

public interface IJwt
{
	public string GenerateAccessToken(Guid id, Role role);

	public string GenerateRefreshToken();

	public Task<Guid> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

	public int GetRefreshTokenExpirationDays();
}
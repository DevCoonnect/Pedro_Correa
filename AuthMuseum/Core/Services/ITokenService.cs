using System.Security.Claims;

namespace AuthMuseum.Core.Services;

public interface ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    public string GenerateRefreshToken();
    public bool TryGetClaimsFromExpiredToken(string token, out ClaimsPrincipal claimsPrincipal);
    public Task<bool> IsTokenBlacklisted(string jti);
    public Task BlacklistToken(string jti);
}
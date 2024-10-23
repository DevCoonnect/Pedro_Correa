using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthMuseum.Core.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace AuthMuseum.Core.Services.Impl;

public class TokenService(IOptions<JwtOptions> jwtOptions, IRedisDatabase redis) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
        
        var tokenProperties = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.SessionTime),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
        };

        return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenProperties);
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public bool TryGetClaimsFromExpiredToken(string token, out ClaimsPrincipal claimsPrincipal)
    {
        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey)),
            ValidateLifetime = false
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
        
        return securityToken is JwtSecurityToken;
    }

    public async Task<bool> IsTokenBlacklisted(string jti)
    {
        return await redis.ExistsAsync($"blacklist:{jti}");
    }

    public async Task BlacklistToken(string jti)
    {
        await redis.AddAsync($"blacklist:{jti}", true, DateTime.UtcNow.AddMinutes(_jwtOptions.SessionTime));
    }
}
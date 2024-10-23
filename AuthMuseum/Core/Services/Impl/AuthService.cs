using System.Security.Claims;
using AuthMuseum.Core.Common;
using AuthMuseum.Core.Helpers;
using AuthMuseum.Core.Options;
using AuthMuseum.Core.Repository;
using AuthMuseum.Domain.Entities;
using AuthMuseum.Domain.Errors;
using AuthMuseum.Domain.Requests;
using AuthMuseum.Domain.Responses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AuthMuseum.Core.Services.Impl;

public class AuthService(IOptions<JwtOptions> jwtSettings, IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    
    private readonly JwtOptions _jwtSettings = jwtSettings.Value;
   
    // Existe uma forma fácil de invalidar o token anterior? O Blacklist não me parece ser uma boa por aqui :/
    // E salvar o token do usuário também me parece ser uma má ideia.
    public async Task<Result<AuthResponse>> AuthenticateUserAsync(AuthRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return new UnauthorizedError($"User with E-mail '{request.Email}' not found!");
        }

        var hashPassword = request.Password.ToSHA256();
        if (user.Password != hashPassword)
        {
            return new UnauthorizedError("Incorrect password!");
        }

        var claims = GenerateClaimsForUser(user);
        var authCredentials = GenerateAuthCredentials(claims);
        
        user.RefreshToken = authCredentials.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.DaysToExpire);
        await userRepository.SaveChangesAsync();
        
        return authCredentials;
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (!tokenService.TryGetClaimsFromExpiredToken(request.Token, out var claimsPrincipal))
        {
            return new UnauthorizedError("Token is invalid!");
        }

        var jti = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrEmpty(jti))
        {
            return new UnauthorizedError("Can't retrieve the JTI from token!");
        }

        if (await tokenService.IsTokenBlacklisted(jti))
        {
            return new UnauthorizedError("Token is blacklisted!");
        }
        
        var userEmail = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return new UnauthorizedError("Can't retrieve the user's email from token!");
        }
        
        var user = await userRepository.GetByEmailAsync(userEmail);
        if (user == null)
        {
            return new UnauthorizedError("Can't retrieve user from token!");
        }

        if (user.RefreshToken != request.RefreshToken)
        {
            return new UnauthorizedError("Refresh token is invalid!");
        }

        if (user.RefreshTokenExpiry is { } expiry && DateTime.UtcNow > expiry)
        {
            return new UnauthorizedError("Refresh token is expired!");
        }

        var authCredentials = GenerateAuthCredentials(claimsPrincipal.Claims);
        
        user.RefreshToken = authCredentials.RefreshToken;
        await userRepository.SaveChangesAsync();
        
        return authCredentials;
    }
    
    public async Task<Result> LogoutAsync(ClaimsPrincipal claims)
    {
        var jti = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrEmpty(jti))
        {
            return new UnauthorizedError("Can't retrieve the jti from token!");
        }
        
        var email = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return new UnauthorizedError("Can't retrieve the email from token!");
        }
        
        var user = await userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return new UnauthorizedError("Can't retrieve user from token!");
        }
        
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await userRepository.SaveChangesAsync();
        
        await tokenService.BlacklistToken(jti);
        
        return Result.Success;
    }

    private static List<Claim> GenerateClaimsForUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var claims = user.Permissions
            .Select(permission => new Claim("permission", permission.ToString()))
            .ToList();
        
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(ClaimTypes.Role, user.Profile.ToString()));

        return claims;
    }

    private AuthResponse GenerateAuthCredentials(IEnumerable<Claim> claims)
    {
        var refreshToken = tokenService.GenerateRefreshToken();
        var accessToken = tokenService.GenerateAccessToken(claims);
        
        var tokenCreationDate = DateTime.UtcNow;
        var tokenExpiration = tokenCreationDate.AddMinutes(_jwtSettings.SessionTime);
        
        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Authenticated = true,
            CreationDate = tokenCreationDate.ToString("o"),
            ExpirationDate = tokenExpiration.ToString("o")
        };

        return response;
    }
}
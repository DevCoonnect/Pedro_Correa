using System.Security.Claims;
using AuthMuseum.Core.Common;
using AuthMuseum.Domain.Requests;
using AuthMuseum.Domain.Responses;

namespace AuthMuseum.Core.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> AuthenticateUserAsync(AuthRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<Result> LogoutAsync(ClaimsPrincipal claims);
}
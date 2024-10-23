using AuthMuseum.Core.Helpers;
using AuthMuseum.Core.Services;
using AuthMuseum.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMuseum.Application.Controllers;

[ApiController]
[Route("api")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request)
    {
        var result = await authService.AuthenticateUserAsync(request);

        return result.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }

    [HttpPost("logout")]
    [Authorize("Bearer")]
    public async Task<IActionResult> Logout()
    {
        var result = await authService.LogoutAsync(User);

        return result.Match(
            NoContent,
            error => error.ErrorToActionResult()
        );
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await authService.RefreshTokenAsync(request);

        return result.Match(
            Ok,
            error => error.ErrorToActionResult()
        );
    }
}